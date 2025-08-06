#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;

#endregion

namespace CinemaTicketingSystem.Persistence.Ticketing;

public class SeatHoldRepository(AppDbContext context) : GenericRepository<SeatHold>(context), ISeatHoldRepository;