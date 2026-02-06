using FitJournal.Core.Enums;

namespace FitJournal.Core.Entities;

/// <summary>
/// Represents an exercise in the exercise library
/// </summary>
public class Exercise : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Instructions { get; set; }
    public ExerciseCategory Category { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public string? VideoUrl { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsCustom { get; set; }
    public string? CreatedByUserId { get; set; }
    
    // Self-referencing for exercise variations
    public int? ParentExerciseId { get; set; }
    public Exercise? ParentExercise { get; set; }
    public ICollection<Exercise> Variations { get; set; } = new List<Exercise>();
    
    // Many-to-many with muscle groups
    public ICollection<ExerciseMuscleGroup> ExerciseMuscleGroups { get; set; } = new List<ExerciseMuscleGroup>();
    
    // Navigation to exercise logs
    public ICollection<ExerciseLog> ExerciseLogs { get; set; } = new List<ExerciseLog>();
}

/// <summary>
/// Join entity for Exercise and MuscleGroup many-to-many relationship
/// </summary>
public class ExerciseMuscleGroup
{
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;
    
    public MuscleGroup MuscleGroup { get; set; }
    public bool IsPrimary { get; set; }
}
