namespace CinemaTicketingSystem.Domain.Repositories
{
    public interface IUnitOfWork
    {

        Task<int> SaveChangesAsync();

    }
}
