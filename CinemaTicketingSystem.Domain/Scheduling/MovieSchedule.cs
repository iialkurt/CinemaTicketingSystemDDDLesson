namespace CinemaTicketingSystem.Domain.Scheduling;

public class MovieSchedule : AuditedAggregateRoot<Guid>
{
    private readonly List<ShowTime> showTimes = [];

    public Guid MovieId { get; private set; }
    public Guid CinemaHallId { get; private set; }
    public virtual IReadOnlyCollection<ShowTime> ShowTimes => showTimes.AsReadOnly();
}