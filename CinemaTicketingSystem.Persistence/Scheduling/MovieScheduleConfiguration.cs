using CinemaTicketingSystem.Domain.Scheduling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Scheduling;

public class MovieScheduleConfiguration : IEntityTypeConfiguration<MovieSchedule>
{
    public void Configure(EntityTypeBuilder<MovieSchedule> builder)
    {
        // Table configuration
        builder.ToTable("MovieSchedules", "scheduling");

        // Primary key
        builder.HasKey(ms => ms.Id);

        // Properties
        builder.Property(ms => ms.Id)
            .ValueGeneratedNever();

        builder.Property(ms => ms.MovieId)
            .IsRequired();

        // Audit properties (inherited from AuditedAggregateRoot)
        builder.Property(ms => ms.CreationTime)
            .IsRequired();

        builder.Property(ms => ms.CreatorId)
            .IsRequired();

        builder.Property(ms => ms.LastModificationTime);

        builder.Property(ms => ms.LastModifierId);

        builder.Property(ms => ms.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(ms => ms.DeleterId);

        builder.Property(ms => ms.DeletionTime);

        // Relationships
        builder.HasMany(x => x.ShowTimes).WithOne(x => x.MovieSchedule);

        builder.HasQueryFilter(ms => !ms.IsDeleted);
    }
}