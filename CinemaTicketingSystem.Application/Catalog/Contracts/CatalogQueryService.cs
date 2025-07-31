using CinemaTicketingSystem.Application.Abstraction;

namespace CinemaTicketingSystem.Application.Catalog.Contracts;

public class CatalogQueryService
{
  public AppResult  GetCinemaInfo(Guid cinemaId)
  {
    // Implementation to retrieve cinema information by ID
    // This is a placeholder for the actual implementation
    return AppResult.SuccessAsNoContent();
  }
}
