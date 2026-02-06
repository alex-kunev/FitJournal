using FitJournal.Core.Entities;
using FitJournal.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Core entities
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<ExerciseMuscleGroup> ExerciseMuscleGroups => Set<ExerciseMuscleGroup>();
    public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();
    public DbSet<ExerciseLog> ExerciseLogs => Set<ExerciseLog>();
    public DbSet<SetLog> SetLogs => Set<SetLog>();

    // Goals (TPH)
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<WeightGoal> WeightGoals => Set<WeightGoal>();
    public DbSet<StrengthGoal> StrengthGoals => Set<StrengthGoal>();
    public DbSet<CardioGoal> CardioGoals => Set<CardioGoal>();
    public DbSet<HabitGoal> HabitGoals => Set<HabitGoal>();
    public DbSet<GoalProgress> GoalProgress => Set<GoalProgress>();

    // Body & Social
    public DbSet<BodyMeasurement> BodyMeasurements => Set<BodyMeasurement>();
    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<UserAchievement> UserAchievements => Set<UserAchievement>();
    public DbSet<ActivityFeedItem> ActivityFeedItems => Set<ActivityFeedItem>();
    public DbSet<UserFollow> UserFollows => Set<UserFollow>();
    public DbSet<WorkoutStreak> WorkoutStreaks => Set<WorkoutStreak>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply all configurations from this assembly
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // ExerciseMuscleGroup - composite key
        builder.Entity<ExerciseMuscleGroup>()
            .HasKey(emg => new { emg.ExerciseId, emg.MuscleGroup });

        // Exercise - self-referencing relationship
        builder.Entity<Exercise>()
            .HasOne(e => e.ParentExercise)
            .WithMany(e => e.Variations)
            .HasForeignKey(e => e.ParentExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Goal - TPH configuration
        builder.Entity<Goal>()
            .HasDiscriminator(g => g.Type)
            .HasValue<WeightGoal>(GoalType.Weight)
            .HasValue<StrengthGoal>(GoalType.Strength)
            .HasValue<CardioGoal>(GoalType.Cardio)
            .HasValue<HabitGoal>(GoalType.Habit);

        // StrengthGoal - Exercise relationship
        builder.Entity<StrengthGoal>()
            .HasOne(sg => sg.Exercise)
            .WithMany()
            .HasForeignKey(sg => sg.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserAchievement - composite key
        builder.Entity<UserAchievement>()
            .HasKey(ua => new { ua.UserId, ua.AchievementId });

        // UserFollow - composite key (self-referencing many-to-many)
        builder.Entity<UserFollow>()
            .HasKey(uf => new { uf.FollowerId, uf.FollowingId });

        // Indexes for performance
        builder.Entity<WorkoutSession>()
            .HasIndex(ws => ws.UserId);

        builder.Entity<WorkoutSession>()
            .HasIndex(ws => ws.StartTime);

        builder.Entity<BodyMeasurement>()
            .HasIndex(bm => new { bm.UserId, bm.Type, bm.RecordedAt });

        builder.Entity<Goal>()
            .HasIndex(g => new { g.UserId, g.Status });

        builder.Entity<ActivityFeedItem>()
            .HasIndex(afi => new { afi.UserId, afi.CreatedAt });

        builder.Entity<WorkoutStreak>()
            .HasIndex(ws => ws.UserId)
            .IsUnique();

        // Seed exercise data
        ExerciseSeeder.SeedExercises(builder);
    }
}

/// <summary>
/// Extended Identity user with fitness profile
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public decimal? Height { get; set; } // in cm
    public WeightUnit PreferredWeightUnit { get; set; } = WeightUnit.Kilograms;
    public DistanceUnit PreferredDistanceUnit { get; set; } = DistanceUnit.Kilometers;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsPublicProfile { get; set; }
    public string? Bio { get; set; }
}
