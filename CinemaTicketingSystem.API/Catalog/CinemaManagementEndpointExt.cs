using Asp.Versioning.Builder;
using CinemaTicketingSystem.API.Catalog.Cinema.Create;
using CinemaTicketingSystem.API.Catalog.Cinema.Hall.Add;
using CinemaTicketingSystem.API.Catalog.Movie.Create;
using CinemaTicketingSystem.API.CinemaManagement.Cinema.GetAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CinemaTicketingSystem.API.Catalog;

public static class CinemaManagementEndpointExt
{
    public static void AddCinemaManagementGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/cinema-management").WithTags("cinema-management")
            .WithApiVersionSet(apiVersionSet)
            .CreateMovieGroupItemEndpoint()
            .CreateCinemaGroupItemEndpoint()
            .GetAllCinemaGroupItemEndpoint()
            .AddCinemaHallGroupItemEndpoint();
    }
}