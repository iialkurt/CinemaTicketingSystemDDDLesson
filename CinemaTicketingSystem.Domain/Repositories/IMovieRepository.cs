using CinemaTicketingSystem.Domain.Catalog;

namespace CinemaTicketingSystem.Domain.Repositories;

public interface IMovieRepository : IGenericRepository<Guid, Movie>
{
    Task<bool> CheckIfMovieExists(string title);
}