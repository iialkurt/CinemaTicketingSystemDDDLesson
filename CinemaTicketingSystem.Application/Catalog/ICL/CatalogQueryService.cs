using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Catalog.ICL.Dto;
using CinemaTicketingSystem.Domain.Catalog.Repositories;
using CinemaTicketingSystem.Domain.Core;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CinemaTicketingSystem.Application.Catalog.ICL;

public class CatalogQueryService(ICinemaRepository cinemaRepository, IMovieRepository movieRepository, AppDependencyService appDependencyService, ILogger<CatalogQueryService> logger) : ICatalogQueryService
{
    public async Task<AppResult<GetCatalogInfoResponse>> GetCinemaInfo(Guid cinemaId, Guid HallId, Guid MovieId)
    {

        var movie = await movieRepository.GetByIdAsync(MovieId);

        if (movie is null)
        {

            logger.LogWarning($"Movie with Id: {MovieId} not found.");
            return appDependencyService.Error<GetCatalogInfoResponse>(ErrorCodes.MovieNotFound, HttpStatusCode.NotFound);
        }

        var cinema = await cinemaRepository.GetByIdAsync(cinemaId);

        if (cinema is null)
        {
            logger.LogWarning($"Cinema with Id: {cinemaId} not found.");
            return appDependencyService.Error<GetCatalogInfoResponse>(ErrorCodes.CinemaNotFound, HttpStatusCode.NotFound);
        }

        var hall = cinema.Halls.FirstOrDefault(h => h.Id == HallId);

        if (hall is null)
        {
            logger.LogWarning($"Cinema hall with Id: {HallId} not found in cinema with Id: {cinemaId}.");
            return appDependencyService.Error<GetCatalogInfoResponse>(ErrorCodes.CinemaHallNotFound, HttpStatusCode.NotFound);
        }


        return AppResult<GetCatalogInfoResponse>.SuccessAsOk(new GetCatalogInfoResponse(cinema.Name, hall.Name, movie.Title, hall.Capacity));
    }
}