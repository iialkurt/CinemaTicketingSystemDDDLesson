using CinemaTicketingSystem.Domain.CinemaManagement;
using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Persistence.CinemaManagement
{
    public class CinemaRepository(AppDbContext context) : GenericRepository<Guid, Cinema>(context), ICinemaRepository

    {




    }
}
