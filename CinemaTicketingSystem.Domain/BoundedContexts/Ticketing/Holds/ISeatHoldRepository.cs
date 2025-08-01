using CinemaTicketingSystem.Domain.Catalog;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.Domain.ValueObjects;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;

public interface ISeatHoldRepository : IGenericRepository<Guid, SeatHold>
{

}
