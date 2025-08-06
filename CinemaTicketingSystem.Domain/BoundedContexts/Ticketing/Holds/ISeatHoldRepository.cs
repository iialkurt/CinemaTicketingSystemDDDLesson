#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;

public interface ISeatHoldRepository : IGenericRepository<SeatHold>
{
}