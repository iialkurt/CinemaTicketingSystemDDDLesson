using CinemaTicketingSystem.Domain.Ticketing.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Ticketing.Configurations;

internal class ReservationTicketConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("SeatReservations", "Ticketing");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.MovieSessionId).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.ReservationTime).IsRequired();
        builder.Property(x => x.ExpirationTime).IsRequired();
        builder.Property(x => x.Status).IsRequired();


        builder.HasMany(x => x.ReservedSeats).WithOne(y => y.Reservation);

        builder.Metadata.FindNavigation(nameof(Reservation.ReservedSeats))!.SetField("reservedSeats");
    }
}