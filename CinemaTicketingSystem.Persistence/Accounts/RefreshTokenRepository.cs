#region

using CinemaTicketingSystem.Domain.BoundedContexts.Accounts;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Accounts;

public class RefreshTokenRepository(AppDbContext context)
    : GenericRepository<RefreshToken>(context), IRefreshTokenRepository;