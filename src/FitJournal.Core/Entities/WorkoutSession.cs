namespace FitJournal.Core.Entities;

/// <summary>
/// Represents a single workout session
/// </summary>
public class WorkoutSession : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Notes { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan? Duration => EndTime.HasValue ? EndTime.Value - StartTime : null;
    public int? CaloriesBurned { get; set; }
    public int? Rating { get; set; } // 1-5 rating
    public bool IsCompleted { get; set; }
    
    // Navigation properties
    public ICollection<ExerciseLog> ExerciseLogs { get; set; } = new List<ExerciseLog>();
}

/// <summary>
/// Represents an exercise performed during a workout session
/// </summary>
public class ExerciseLog : BaseEntity
{
    public int WorkoutSessionId { get; set; }
    public WorkoutSession WorkoutSession { get; set; } = null!;
    
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;
    
    public int Order { get; set; }
    public string? Notes { get; set; }
    
    // Navigation to individual sets
    public ICollection<SetLog> Sets { get; set; } = new List<SetLog>();
}

/// <summary>
/// Represents a single set within an exercise log
/// </summary>
public class SetLog : BaseEntity
{
    public int ExerciseLogId { get; set; }
    public ExerciseLog ExerciseLog { get; set; } = null!;
    
    public int SetNumber { get; set; }
    public int? Reps { get; set; }
    public decimal? Weight { get; set; }
    public TimeSpan? Duration { get; set; }
    public decimal? Distance { get; set; }
    public bool IsWarmup { get; set; }
    public bool IsDropSet { get; set; }
    public bool IsCompleted { get; set; }
    public string? Notes { get; set; }
}
