using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Ticketing.Configurations;

internal class SeatHold : IEntityTypeConfiguration<ReservationSeat>
{
    public void Configure(EntityTypeBuilder<ReservationSeat> builder)
    {
        builder.ToTable("ReservedSeats", "Ticketing");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.OwnsOne(x => x.SeatPosition, seatBuilder =>
        {
            seatBuilder.Property(s => s.Number)
                .HasColumnName("Number")
                .IsRequired();

            seatBuilder.Property(s => s.Row)
                .HasColumnName("Row")
                .HasMaxLength(1).IsFixedLength()
                .IsRequired().IsUnicode(false);
        });
    }
}