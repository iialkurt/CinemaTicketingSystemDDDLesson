namespace CinemaTicketingSystem.Domain.CinemaManagement;

public class Seat : Entity<Guid>
{
    public string Row { get; private set; } = null!;
    public int Number { get; private set; }
    public SeatType Type { get; private set; } // Enum: Regular, VIP, Accessible
}