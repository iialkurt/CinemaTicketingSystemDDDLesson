#region

using CinemaTicketingSystem.Domain.BoundedContexts.Accounts;

#endregion

namespace CinemaTicketingSystem.Persistence.Accounts;

public class RefreshTokenRepository(AppDbContext context)
    : GenericRepository<RefreshToken>(context), IRefreshTokenRepository;