using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Catalog.ICL.Dto;

namespace CinemaTicketingSystem.Application.Catalog.ICL;

public interface ICatalogQueryService
{
    Task<AppResult<GetCatalogInfoResponse>> GetCinemaInfo(Guid cinemaId, Guid HallId, Guid MovieId);
}