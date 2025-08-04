using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;

namespace CinemaTicketingSystem.Persistence.Ticketing;

public class ReservationRepository(AppDbContext context)
    : GenericRepository<Reservation>(context), IReservationRepository;