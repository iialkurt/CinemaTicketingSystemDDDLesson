using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Repositories;

public interface ICinemaRepository : IGenericRepository<Cinema>
{
    Task<Cinema?> GetByHallId(Guid hallId);
}