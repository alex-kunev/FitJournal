using FitJournal.Core.Entities;
using FitJournal.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Infrastructure.Data;

public static class ExerciseSeeder
{
    public static void SeedExercises(ModelBuilder builder)
    {
        var exercises = new List<Exercise>
        {
            // Chest exercises
            new() { Id = 1, Name = "Bench Press", Description = "Classic barbell chest press", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Intermediate, Instructions = "Lie on bench, lower bar to chest, press up" },
            new() { Id = 2, Name = "Push-ups", Description = "Bodyweight chest exercise", Category = ExerciseCategory.Calisthenics, Difficulty = DifficultyLevel.Beginner, Instructions = "Start in plank, lower chest to ground, push back up" },
            new() { Id = 3, Name = "Dumbbell Flyes", Description = "Chest isolation exercise", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Intermediate, Instructions = "Lie on bench, arms out wide, bring dumbbells together" },
            new() { Id = 4, Name = "Incline Bench Press", Description = "Upper chest focus", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Intermediate, ParentExerciseId = 1, Instructions = "Same as bench press but on inclined bench" },
            
            // Back exercises
            new() { Id = 5, Name = "Pull-ups", Description = "Upper back bodyweight exercise", Category = ExerciseCategory.Calisthenics, Difficulty = DifficultyLevel.Intermediate, Instructions = "Hang from bar, pull chin over bar" },
            new() { Id = 6, Name = "Barbell Rows", Description = "Compound back exercise", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Intermediate, Instructions = "Bend at hips, pull bar to lower chest" },
            new() { Id = 7, Name = "Lat Pulldowns", Description = "Machine back exercise", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Beginner, Instructions = "Pull bar down to chest, control on way up" },
            new() { Id = 8, Name = "Deadlifts", Description = "Full posterior chain exercise", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Advanced, Instructions = "Lift barbell from floor by extending hips and knees" },
            
            // Legs
            new() { Id = 9, Name = "Squats", Description = "King of leg exercises", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Intermediate, Instructions = "Feet shoulder width, squat down, drive through heels" },
            new() { Id = 10, Name = "Lunges", Description = "Single leg exercise", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Beginner, Instructions = "Step forward, lower back knee, push back up" },
            new() { Id = 11, Name = "Leg Press", Description = "Machine quad exercise", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Beginner, Instructions = "Push platform away, control on return" },
            new() { Id = 12, Name = "Romanian Deadlift", Description = "Hamstring focus", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Intermediate, Instructions = "Hinge at hips with slight knee bend" },
            
            // Shoulders
            new() { Id = 13, Name = "Overhead Press", Description = "Shoulder compound movement", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Intermediate, Instructions = "Press barbell from shoulders overhead" },
            new() { Id = 14, Name = "Lateral Raises", Description = "Side delt isolation", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Beginner, Instructions = "Raise dumbbells to sides until parallel" },
            new() { Id = 15, Name = "Face Pulls", Description = "Rear delt and rotator cuff", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Beginner, Instructions = "Pull rope to face, squeeze rear delts" },
            
            // Arms
            new() { Id = 16, Name = "Bicep Curls", Description = "Bicep isolation", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Beginner, Instructions = "Curl weight up, squeeze at top" },
            new() { Id = 17, Name = "Tricep Dips", Description = "Tricep bodyweight exercise", Category = ExerciseCategory.Calisthenics, Difficulty = DifficultyLevel.Intermediate, Instructions = "Lower body between parallel bars, push up" },
            new() { Id = 18, Name = "Hammer Curls", Description = "Bicep and forearm", Category = ExerciseCategory.Strength, Difficulty = DifficultyLevel.Beginner, ParentExerciseId = 16, Instructions = "Curl with neutral grip" },
            
            // Core
            new() { Id = 19, Name = "Plank", Description = "Core stabilization", Category = ExerciseCategory.Calisthenics, Difficulty = DifficultyLevel.Beginner, Instructions = "Hold pushup position, keep body straight" },
            new() { Id = 20, Name = "Crunches", Description = "Ab isolation", Category = ExerciseCategory.Calisthenics, Difficulty = DifficultyLevel.Beginner, Instructions = "Lift shoulders off ground, squeeze abs" },
            new() { Id = 21, Name = "Russian Twists", Description = "Oblique exercise", Category = ExerciseCategory.Calisthenics, Difficulty = DifficultyLevel.Intermediate, Instructions = "Seated, rotate torso side to side" },
            
            // Cardio
            new() { Id = 22, Name = "Running", Description = "Classic cardio", Category = ExerciseCategory.Cardio, Difficulty = DifficultyLevel.Beginner, Instructions = "Maintain steady pace, proper form" },
            new() { Id = 23, Name = "Cycling", Description = "Low impact cardio", Category = ExerciseCategory.Cardio, Difficulty = DifficultyLevel.Beginner, Instructions = "Adjust seat height, maintain cadence" },
            new() { Id = 24, Name = "Jump Rope", Description = "High intensity cardio", Category = ExerciseCategory.Cardio, Difficulty = DifficultyLevel.Intermediate, Instructions = "Jump with both feet, light on toes" },
            new() { Id = 25, Name = "Burpees", Description = "Full body cardio", Category = ExerciseCategory.Plyometrics, Difficulty = DifficultyLevel.Intermediate, Instructions = "Squat, kick back, pushup, jump up" },
        };

        builder.Entity<Exercise>().HasData(exercises);

        // Seed muscle group associations
        var muscleGroups = new List<ExerciseMuscleGroup>
        {
            // Chest exercises
            new() { ExerciseId = 1, MuscleGroup = MuscleGroup.Chest, IsPrimary = true },
            new() { ExerciseId = 1, MuscleGroup = MuscleGroup.Triceps, IsPrimary = false },
            new() { ExerciseId = 2, MuscleGroup = MuscleGroup.Chest, IsPrimary = true },
            new() { ExerciseId = 2, MuscleGroup = MuscleGroup.Triceps, IsPrimary = false },
            new() { ExerciseId = 3, MuscleGroup = MuscleGroup.Chest, IsPrimary = true },
            new() { ExerciseId = 4, MuscleGroup = MuscleGroup.Chest, IsPrimary = true },
            
            // Back exercises
            new() { ExerciseId = 5, MuscleGroup = MuscleGroup.Back, IsPrimary = true },
            new() { ExerciseId = 5, MuscleGroup = MuscleGroup.Biceps, IsPrimary = false },
            new() { ExerciseId = 6, MuscleGroup = MuscleGroup.Back, IsPrimary = true },
            new() { ExerciseId = 7, MuscleGroup = MuscleGroup.Back, IsPrimary = true },
            new() { ExerciseId = 8, MuscleGroup = MuscleGroup.Back, IsPrimary = true },
            new() { ExerciseId = 8, MuscleGroup = MuscleGroup.Hamstrings, IsPrimary = false },
            new() { ExerciseId = 8, MuscleGroup = MuscleGroup.Glutes, IsPrimary = false },
            
            // Legs
            new() { ExerciseId = 9, MuscleGroup = MuscleGroup.Quadriceps, IsPrimary = true },
            new() { ExerciseId = 9, MuscleGroup = MuscleGroup.Glutes, IsPrimary = false },
            new() { ExerciseId = 10, MuscleGroup = MuscleGroup.Quadriceps, IsPrimary = true },
            new() { ExerciseId = 11, MuscleGroup = MuscleGroup.Quadriceps, IsPrimary = true },
            new() { ExerciseId = 12, MuscleGroup = MuscleGroup.Hamstrings, IsPrimary = true },
            
            // Shoulders
            new() { ExerciseId = 13, MuscleGroup = MuscleGroup.Shoulders, IsPrimary = true },
            new() { ExerciseId = 14, MuscleGroup = MuscleGroup.Shoulders, IsPrimary = true },
            new() { ExerciseId = 15, MuscleGroup = MuscleGroup.Shoulders, IsPrimary = true },
            
            // Arms
            new() { ExerciseId = 16, MuscleGroup = MuscleGroup.Biceps, IsPrimary = true },
            new() { ExerciseId = 17, MuscleGroup = MuscleGroup.Triceps, IsPrimary = true },
            new() { ExerciseId = 18, MuscleGroup = MuscleGroup.Biceps, IsPrimary = true },
            new() { ExerciseId = 18, MuscleGroup = MuscleGroup.Forearms, IsPrimary = false },
            
            // Core
            new() { ExerciseId = 19, MuscleGroup = MuscleGroup.Abs, IsPrimary = true },
            new() { ExerciseId = 20, MuscleGroup = MuscleGroup.Abs, IsPrimary = true },
            new() { ExerciseId = 21, MuscleGroup = MuscleGroup.Obliques, IsPrimary = true },
            
            // Cardio
            new() { ExerciseId = 22, MuscleGroup = MuscleGroup.Cardio, IsPrimary = true },
            new() { ExerciseId = 23, MuscleGroup = MuscleGroup.Cardio, IsPrimary = true },
            new() { ExerciseId = 24, MuscleGroup = MuscleGroup.Cardio, IsPrimary = true },
            new() { ExerciseId = 25, MuscleGroup = MuscleGroup.FullBody, IsPrimary = true },
        };

        builder.Entity<ExerciseMuscleGroup>().HasData(muscleGroups);
    }
}
