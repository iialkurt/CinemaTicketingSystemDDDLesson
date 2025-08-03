using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Repositories;

public interface IMovieRepository : IGenericRepository<Movie>
{
    Task<bool> CheckIfMovieExists(string title);
}