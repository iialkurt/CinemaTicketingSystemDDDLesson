namespace CinemaTicketingSystem.Domain.CinemaManagement;

public class Cinema : AuditedAggregateRoot<Guid>
{
    private readonly List<CinemaHall> cinemaHalls = [];

    public new Guid Id { get; private set; }
    public string? Name { get; private set; }
    public Address? Address { get; private set; }
    public IReadOnlyCollection<CinemaHall> Halls => cinemaHalls.AsReadOnly();
}