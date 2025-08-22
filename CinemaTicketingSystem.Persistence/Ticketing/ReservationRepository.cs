#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Ticketing;

public class ReservationRepository(AppDbContext context)
    : GenericRepository<Reservation>(context), IReservationRepository;