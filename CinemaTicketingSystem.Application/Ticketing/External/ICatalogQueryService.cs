#region

using CinemaTicketingSystem.Application.Abstraction;

#endregion

namespace CinemaTicketingSystem.Application.Ticketing.External;

public interface ICatalogQueryService
{
    Task<AppResult<GetCatalogInfoResponse>> GetCinemaInfo(Guid hallId, Guid movieId);
}