using FitJournal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitJournal.Infrastructure.Data.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.Instructions)
            .HasMaxLength(2000);

        builder.Property(e => e.VideoUrl)
            .HasMaxLength(500);

        builder.Property(e => e.ImageUrl)
            .HasMaxLength(500);

        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.Category);
    }
}

public class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.Property(ws => ws.Name)
            .HasMaxLength(100);

        builder.Property(ws => ws.Notes)
            .HasMaxLength(1000);

        builder.Property(ws => ws.UserId)
            .HasMaxLength(450)
            .IsRequired();
    }
}

public class ExerciseLogConfiguration : IEntityTypeConfiguration<ExerciseLog>
{
    public void Configure(EntityTypeBuilder<ExerciseLog> builder)
    {
        builder.Property(el => el.Notes)
            .HasMaxLength(500);

        builder.HasOne(el => el.WorkoutSession)
            .WithMany(ws => ws.ExerciseLogs)
            .HasForeignKey(el => el.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(el => el.Exercise)
            .WithMany(e => e.ExerciseLogs)
            .HasForeignKey(el => el.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class SetLogConfiguration : IEntityTypeConfiguration<SetLog>
{
    public void Configure(EntityTypeBuilder<SetLog> builder)
    {
        builder.Property(sl => sl.Weight)
            .HasPrecision(10, 2);

        builder.Property(sl => sl.Distance)
            .HasPrecision(10, 2);

        builder.Property(sl => sl.Notes)
            .HasMaxLength(200);

        builder.HasOne(sl => sl.ExerciseLog)
            .WithMany(el => el.Sets)
            .HasForeignKey(sl => sl.ExerciseLogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
