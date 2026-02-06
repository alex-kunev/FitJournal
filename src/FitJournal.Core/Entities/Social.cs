using FitJournal.Core.Enums;

namespace FitJournal.Core.Entities;

/// <summary>
/// Body measurement record
/// </summary>
public class BodyMeasurement : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public MeasurementType Type { get; set; }
    public decimal Value { get; set; }
    public DateTime RecordedAt { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Achievement/badge that can be earned
/// </summary>
public class Achievement : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Points { get; set; }
    public string? RequirementDescription { get; set; }
    
    // Navigation
    public ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}

/// <summary>
/// Join entity for User and Achievement
/// </summary>
public class UserAchievement
{
    public string UserId { get; set; } = string.Empty;
    public int AchievementId { get; set; }
    public Achievement Achievement { get; set; } = null!;
    public DateTime EarnedAt { get; set; }
}

/// <summary>
/// Activity feed item for social features
/// </summary>
public class ActivityFeedItem : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string ActivityType { get; set; } = string.Empty; // "workout_completed", "goal_achieved", etc.
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? RelatedEntityType { get; set; }
    public int? RelatedEntityId { get; set; }
    public bool IsPublic { get; set; }
}

/// <summary>
/// User follow relationship for social features
/// </summary>
public class UserFollow
{
    public string FollowerId { get; set; } = string.Empty;
    public string FollowingId { get; set; } = string.Empty;
    public DateTime FollowedAt { get; set; }
}

/// <summary>
/// Workout streak tracking
/// </summary>
public class WorkoutStreak : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateTime? LastWorkoutDate { get; set; }
    public DateTime? StreakStartDate { get; set; }
}
