namespace CinemaTicketingSystem.Domain.CinemaManagement;

public class CinemaHall : Entity<Guid>
{
    private readonly List<Seat> seats = [];

    public string? Name { get; private set; }
    public int Capacity => Seats.Count;
    public IReadOnlyList<Seat> Seats => seats.AsReadOnly();
}