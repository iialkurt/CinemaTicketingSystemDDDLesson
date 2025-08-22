#region

using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.Infrastructure.Persistence;

#endregion

namespace CinemaTicketingSystem.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}