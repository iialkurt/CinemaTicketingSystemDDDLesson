using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Persistence;

internal class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}