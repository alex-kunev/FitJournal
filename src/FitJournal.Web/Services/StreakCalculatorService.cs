using FitJournal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Web.Services;

public class StreakCalculatorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StreakCalculatorService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);

    public StreakCalculatorService(IServiceProvider serviceProvider, ILogger<StreakCalculatorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Streak Calculator Service starting");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CalculateAllStreaks();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating streaks");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task CalculateAllStreaks()
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var users = await dbContext.Users.ToListAsync();

        foreach (var user in users)
        {
            await CalculateUserStreak(dbContext, user.Id);
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task CalculateUserStreak(ApplicationDbContext dbContext, string userId)
    {
        var workouts = await dbContext.WorkoutSessions
            .Where(w => w.UserId == userId && w.IsCompleted)
            .OrderByDescending(w => w.EndTime)
            .Select(w => w.EndTime!.Value.Date)
            .Distinct()
            .ToListAsync();

        if (!workouts.Any()) return;

        var today = DateTime.UtcNow.Date;
        var streak = 0;
        var maxStreak = 0;
        var currentDate = today;

        // Check if there's a workout today or yesterday to continue the streak
        if (!workouts.Contains(today) && !workouts.Contains(today.AddDays(-1)))
        {
            // Streak is broken
            streak = 0;
        }
        else
        {
            // Calculate current streak
            foreach (var date in workouts)
            {
                if (date == currentDate || date == currentDate.AddDays(-1))
                {
                    streak++;
                    currentDate = date;
                }
                else
                {
                    break;
                }
            }
        }

        // Calculate max streak (simplified - just track current for now)
        maxStreak = Math.Max(streak, maxStreak);

        // Update or create workout streak record
        var existingStreak = await dbContext.WorkoutStreaks
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (existingStreak != null)
        {
            existingStreak.CurrentStreak = streak;
            existingStreak.LongestStreak = Math.Max(existingStreak.LongestStreak, streak);
            existingStreak.LastWorkoutDate = workouts.FirstOrDefault();
        }
        else
        {
            dbContext.WorkoutStreaks.Add(new Core.Entities.WorkoutStreak
            {
                UserId = userId,
                CurrentStreak = streak,
                LongestStreak = streak,
                LastWorkoutDate = workouts.FirstOrDefault()
            });
        }

        _logger.LogDebug("User {UserId} streak: {Streak} days", userId, streak);
    }
}
