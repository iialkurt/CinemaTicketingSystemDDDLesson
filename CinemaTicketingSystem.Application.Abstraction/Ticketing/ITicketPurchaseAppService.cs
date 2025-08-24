#region

using CinemaTicketingSystem.Application.Abstraction;

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public interface ITicketPurchaseAppService
{
    Task<AppResult> Create(CreateTicketIssuanceRequest request);

    Task<AppResult> CreateFromReservation(Guid reservationId);
}