#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;

public interface IReservationRepository : IGenericRepository<Reservation>
{
}