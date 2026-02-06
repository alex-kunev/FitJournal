using FitJournal.Core.Entities;
using FitJournal.Core.Enums;
using FitJournal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Web.Services;

public class GoalMilestoneService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GoalMilestoneService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);

    public GoalMilestoneService(IServiceProvider serviceProvider, ILogger<GoalMilestoneService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Goal Milestone Service starting");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckGoalMilestones();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking goal milestones");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task CheckGoalMilestones()
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Check weight goals against latest measurements
        var weightGoals = await dbContext.Goals
            .OfType<WeightGoal>()
            .Where(g => g.Status == GoalStatus.Active)
            .ToListAsync();

        foreach (var goal in weightGoals)
        {
            var latestWeight = await dbContext.BodyMeasurements
                .Where(m => m.UserId == goal.UserId && m.Type == MeasurementType.Weight)
                .OrderByDescending(m => m.RecordedAt)
                .FirstOrDefaultAsync();

            if (latestWeight != null)
            {
                goal.CurrentWeight = latestWeight.Value;

                // Check if goal is achieved
                var isAchieved = goal.StartWeight > goal.TargetWeight
                    ? latestWeight.Value <= goal.TargetWeight // Weight loss
                    : latestWeight.Value >= goal.TargetWeight; // Weight gain

                if (isAchieved && goal.Status == GoalStatus.Active)
                {
                    goal.Status = GoalStatus.Completed;
                    goal.CompletedDate = DateTime.UtcNow;

                    dbContext.ActivityFeedItems.Add(new ActivityFeedItem
                    {
                        UserId = goal.UserId,
                        ActivityType = "goal_achieved",
                        Title = $"🎯 Goal Achieved: {goal.Name}",
                        Description = $"Reached target weight of {goal.TargetWeight} {goal.Unit}!",
                        IsPublic = true
                    });

                    _logger.LogInformation("User {UserId} achieved weight goal: {Goal}", goal.UserId, goal.Name);
                }
            }
        }

        // Check strength goals against workout logs
        var strengthGoals = await dbContext.Goals
            .OfType<StrengthGoal>()
            .Where(g => g.Status == GoalStatus.Active)
            .ToListAsync();

        foreach (var goal in strengthGoals)
        {
            var bestLift = await dbContext.SetLogs
                .Include(s => s.ExerciseLog)
                    .ThenInclude(e => e.WorkoutSession)
                .Include(s => s.ExerciseLog)
                    .ThenInclude(e => e.Exercise)
                .Where(s => s.ExerciseLog.WorkoutSession.UserId == goal.UserId 
                         && s.ExerciseLog.ExerciseId == goal.ExerciseId
                         && s.IsCompleted)
                .OrderByDescending(s => s.Weight)
                .FirstOrDefaultAsync();

            if (bestLift != null && bestLift.Weight.HasValue)
            {
                goal.CurrentBest = bestLift.Weight;

                if (bestLift.Weight >= goal.TargetWeight && (bestLift.Reps ?? 0) >= goal.TargetReps)
                {
                    goal.Status = GoalStatus.Completed;
                    goal.CompletedDate = DateTime.UtcNow;

                    dbContext.ActivityFeedItems.Add(new ActivityFeedItem
                    {
                        UserId = goal.UserId,
                        ActivityType = "goal_achieved",
                        Title = $"🎯 Goal Achieved: {goal.Name}",
                        Description = $"Hit {goal.TargetWeight}{goal.Unit} × {goal.TargetReps} reps!",
                        IsPublic = true
                    });

                    _logger.LogInformation("User {UserId} achieved strength goal: {Goal}", goal.UserId, goal.Name);
                }
            }
        }

        await dbContext.SaveChangesAsync();
    }
}
