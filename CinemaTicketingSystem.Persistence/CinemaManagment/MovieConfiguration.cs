using CinemaTicketingSystem.Domain.CinemaManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.CinemaManagment;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        // Table configuration
        builder.ToTable("Movies", "cinema_mgmt");

        // Primary key
        builder.HasKey(m => m.Id);

        // Properties
        builder.Property(m => m.Id)
            .ValueGeneratedNever();

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.OriginalTitle)
            .HasMaxLength(200);

        builder.Property(m => m.Description)
            .HasMaxLength(2000);

        // Owned type for Duration (Value Object)
        builder.OwnsOne(m => m.Duration, duration =>
        {
            duration.Property(d => d.Minutes)
                .IsRequired()
                .HasColumnName("DurationMinutes");

            // Computed columns for convenience
            duration.Ignore(d => d.Hours);
            duration.Ignore(d => d.RemainingMinutes);
        });



    }
}
