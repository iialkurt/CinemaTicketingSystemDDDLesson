namespace CinemaTicketingSystem.Domain.Scheduling;

public class ShowTime : Entity<Guid>
{
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }
}