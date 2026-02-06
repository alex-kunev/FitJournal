using FitJournal.Core.Entities;
using FitJournal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Web.Services;

public class AchievementProcessorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AchievementProcessorService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(30);

    public AchievementProcessorService(IServiceProvider serviceProvider, ILogger<AchievementProcessorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Achievement Processor Service starting");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessAchievements();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing achievements");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task ProcessAchievements()
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var users = await dbContext.Users.ToListAsync();
        var achievements = await dbContext.Achievements.ToListAsync();

        foreach (var user in users)
        {
            await CheckUserAchievements(dbContext, user.Id, achievements);
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task CheckUserAchievements(ApplicationDbContext dbContext, string userId, List<Achievement> achievements)
    {
        var earnedIds = await dbContext.UserAchievements
            .Where(ua => ua.UserId == userId)
            .Select(ua => ua.AchievementId)
            .ToHashSetAsync();

        var workoutCount = await dbContext.WorkoutSessions
            .CountAsync(w => w.UserId == userId && w.IsCompleted);

        var streak = await dbContext.WorkoutStreaks
            .Where(s => s.UserId == userId)
            .Select(s => s.CurrentStreak)
            .FirstOrDefaultAsync();

        var goalCount = await dbContext.Goals
            .CountAsync(g => g.UserId == userId && g.Status == Core.Enums.GoalStatus.Completed);

        var measurementCount = await dbContext.BodyMeasurements
            .CountAsync(m => m.UserId == userId);

        foreach (var achievement in achievements)
        {
            if (earnedIds.Contains(achievement.Id)) continue;

            var earned = achievement.Name switch
            {
                "First Steps" => workoutCount >= 1,
                "Getting Started" => workoutCount >= 5,
                "Dedicated" => workoutCount >= 25,
                "Fitness Fanatic" => workoutCount >= 100,
                "3-Day Streak" => streak >= 3,
                "Week Warrior" => streak >= 7,
                "Two Week Champion" => streak >= 14,
                "Monthly Master" => streak >= 30,
                "Goal Setter" => await dbContext.Goals.AnyAsync(g => g.UserId == userId),
                "Goal Getter" => goalCount >= 1,
                "Ambitious" => goalCount >= 5,
                "Tracker" => measurementCount >= 1,
                _ => false
            };

            if (earned)
            {
                dbContext.UserAchievements.Add(new UserAchievement
                {
                    UserId = userId,
                    AchievementId = achievement.Id,
                    EarnedAt = DateTime.UtcNow
                });

                // Create activity feed item
                dbContext.ActivityFeedItems.Add(new ActivityFeedItem
                {
                    UserId = userId,
                    ActivityType = "achievement_earned",
                    Title = $"🏆 Achievement Unlocked: {achievement.Name}",
                    Description = achievement.Description,
                    IsPublic = true
                });

                _logger.LogInformation("User {UserId} earned achievement: {Achievement}", userId, achievement.Name);
            }
        }
    }
}
