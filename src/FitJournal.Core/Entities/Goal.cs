using FitJournal.Core.Enums;

namespace FitJournal.Core.Entities;

/// <summary>
/// Abstract base class for different types of goals (TPH inheritance)
/// </summary>
public abstract class Goal : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public GoalType Type { get; set; }
    public GoalStatus Status { get; set; } = GoalStatus.Active;
    public DateTime StartDate { get; set; }
    public DateTime? TargetDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    
    // Navigation
    public ICollection<GoalProgress> ProgressEntries { get; set; } = new List<GoalProgress>();
}

/// <summary>
/// Goal for reaching a target weight
/// </summary>
public class WeightGoal : Goal
{
    public decimal StartWeight { get; set; }
    public decimal TargetWeight { get; set; }
    public decimal? CurrentWeight { get; set; }
    public WeightUnit Unit { get; set; } = WeightUnit.Kilograms;
}

/// <summary>
/// Goal for strength achievements (e.g., bench press 100kg)
/// </summary>
public class StrengthGoal : Goal
{
    public int ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }
    public decimal TargetWeight { get; set; }
    public int TargetReps { get; set; }
    public decimal? CurrentBest { get; set; }
    public WeightUnit Unit { get; set; } = WeightUnit.Kilograms;
}

/// <summary>
/// Goal for cardio achievements (e.g., run 5km in under 25 minutes)
/// </summary>
public class CardioGoal : Goal
{
    public decimal? TargetDistance { get; set; }
    public TimeSpan? TargetDuration { get; set; }
    public TimeSpan? TargetPace { get; set; }
    public DistanceUnit DistanceUnit { get; set; } = DistanceUnit.Kilometers;
}

/// <summary>
/// Goal for building habits (e.g., work out 4 times per week)
/// </summary>
public class HabitGoal : Goal
{
    public int TargetFrequency { get; set; } // Times per week
    public int CurrentStreak { get; set; }
    public int BestStreak { get; set; }
}

/// <summary>
/// Tracks progress updates for a goal
/// </summary>
public class GoalProgress : BaseEntity
{
    public int GoalId { get; set; }
    public Goal Goal { get; set; } = null!;
    
    public DateTime RecordedAt { get; set; }
    public decimal? NumericValue { get; set; }
    public string? Notes { get; set; }
}
