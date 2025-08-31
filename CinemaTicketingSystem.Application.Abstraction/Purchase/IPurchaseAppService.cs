using CinemaTicketingSystem.Application.Abstraction;

namespace CinemaTicketingSystem.Application.Contracts.Purchase;

public interface IPurchaseAppService
{
    Task<AppResult> Create(CreatePurchaseRequest request);
}