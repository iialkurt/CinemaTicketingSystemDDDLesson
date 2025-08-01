using CinemaTicketingSystem.Domain.Ticketing.Reservations;
using CinemaTicketingSystem.Domain.ValueObjects;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;

public class ReservedSeat : Entity<Guid>
{
    internal ReservedSeat(SeatNumber seatNumber)
    {
        Id = Guid.CreateVersion7();
        SeatNumber = seatNumber;
    }

    private ReservedSeat()
    {
    }

    public SeatNumber SeatNumber { get; } = null!;

    public virtual Reservation Reservation { get; set; } = null!;

    public string GetSeatInfo()
    {
        return $"Seat: {SeatNumber}";
    }
}
