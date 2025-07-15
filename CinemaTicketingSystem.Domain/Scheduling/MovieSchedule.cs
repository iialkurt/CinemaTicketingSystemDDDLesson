namespace CinemaTicketingSystem.Domain.Scheduling;

public class MovieSchedule : AuditedAggregateRoot<Guid>
{
    private readonly List<ShowTime> showTimes = [];

    private MovieSchedule()
    {
    }

    public Guid MovieId { get; private set; }
    public virtual IReadOnlyCollection<ShowTime> ShowTimes => showTimes.AsReadOnly();

    public void AddShowTime(ShowTime showTime)
    {
        if (showTime == null)
            throw new ArgumentNullException(nameof(showTime));

        if (showTimes.Any(st => st.OverlapsWith(showTime)))
            throw new InvalidOperationException(
                $"Show time {showTime.GetTimeRange()} overlaps with existing show time");

        showTimes.Add(showTime);
    }

    public void AddShowTime(string timeRange)
    {
        var showTime = ShowTime.Create(timeRange);
        AddShowTime(showTime);
    }
}