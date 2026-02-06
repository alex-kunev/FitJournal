using FitJournal.Core.Entities;
using FitJournal.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitJournal.Infrastructure.Data.Configurations;

public class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.Property(g => g.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Description)
            .HasMaxLength(500);

        builder.Property(g => g.UserId)
            .HasMaxLength(450)
            .IsRequired();
    }
}

public class WeightGoalConfiguration : IEntityTypeConfiguration<WeightGoal>
{
    public void Configure(EntityTypeBuilder<WeightGoal> builder)
    {
        builder.Property(wg => wg.StartWeight)
            .HasPrecision(10, 2);

        builder.Property(wg => wg.TargetWeight)
            .HasPrecision(10, 2);

        builder.Property(wg => wg.CurrentWeight)
            .HasPrecision(10, 2);
    }
}

public class StrengthGoalConfiguration : IEntityTypeConfiguration<StrengthGoal>
{
    public void Configure(EntityTypeBuilder<StrengthGoal> builder)
    {
        builder.Property(sg => sg.TargetWeight)
            .HasPrecision(10, 2);

        builder.Property(sg => sg.CurrentBest)
            .HasPrecision(10, 2);
    }
}

public class CardioGoalConfiguration : IEntityTypeConfiguration<CardioGoal>
{
    public void Configure(EntityTypeBuilder<CardioGoal> builder)
    {
        builder.Property(cg => cg.TargetDistance)
            .HasPrecision(10, 2);
    }
}

public class BodyMeasurementConfiguration : IEntityTypeConfiguration<BodyMeasurement>
{
    public void Configure(EntityTypeBuilder<BodyMeasurement> builder)
    {
        builder.Property(bm => bm.Value)
            .HasPrecision(10, 2);

        builder.Property(bm => bm.UserId)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(bm => bm.Notes)
            .HasMaxLength(200);
    }
}

public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
        builder.Property(a => a.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(a => a.Icon)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.Category)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.RequirementDescription)
            .HasMaxLength(200);
    }
}

public class ActivityFeedItemConfiguration : IEntityTypeConfiguration<ActivityFeedItem>
{
    public void Configure(EntityTypeBuilder<ActivityFeedItem> builder)
    {
        builder.Property(afi => afi.UserId)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(afi => afi.ActivityType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(afi => afi.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(afi => afi.Description)
            .HasMaxLength(500);

        builder.Property(afi => afi.RelatedEntityType)
            .HasMaxLength(50);
    }
}

public class WorkoutStreakConfiguration : IEntityTypeConfiguration<WorkoutStreak>
{
    public void Configure(EntityTypeBuilder<WorkoutStreak> builder)
    {
        builder.Property(ws => ws.UserId)
            .HasMaxLength(450)
            .IsRequired();
    }
}
