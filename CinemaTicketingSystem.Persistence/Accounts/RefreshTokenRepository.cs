using CinemaTicketingSystem.Domain.BoundedContexts.Accounts;

namespace CinemaTicketingSystem.Persistence.Accounts
{
    public class RefreshTokenRepository(AppDbContext context)
        : GenericRepository<RefreshToken>(context), IRefreshTokenRepository;
}
