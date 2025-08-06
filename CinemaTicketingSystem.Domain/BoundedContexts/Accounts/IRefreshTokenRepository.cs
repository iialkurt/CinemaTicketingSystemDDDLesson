#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
}