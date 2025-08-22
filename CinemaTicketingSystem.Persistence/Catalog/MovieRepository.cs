#region

using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Repositories;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Catalog;

internal class MovieRepository(AppDbContext context) : GenericRepository<Movie>(context), IMovieRepository
{
    public Task<bool> CheckIfMovieExists(string title)
    {
        return _context.Movies.AnyAsync(m =>
            m.Title.Equals(title));
    }
}