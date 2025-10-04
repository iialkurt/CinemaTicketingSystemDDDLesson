#region

using Asp.Versioning.Builder;
using CinemaTicketingSystem.Presentation.API.Ticketing.Reservation.Confirm;
using CinemaTicketingSystem.Presentation.API.Ticketing.Reservation.Reserve;
using CinemaTicketingSystem.Presentation.API.Ticketing.SeatHold.Cancel;
using CinemaTicketingSystem.Presentation.API.Ticketing.SeatHold.Confirm;
using CinemaTicketingSystem.Presentation.API.Ticketing.SeatHold.Create;
using CinemaTicketingSystem.Presentation.API.Ticketing.TicketIssuance.Create;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing;

public static class TicketingEndpointExt
{
    public static void AddTicketingGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/ticketing").WithTags("ticketing")
            .WithApiVersionSet(apiVersionSet)
            .ReserveSeatsGroupItemEndpoint()
            .ConfirmReservationGroupItemEndpoint()
            .PurchaseTicketsGroupItemEndpoint()
            .CreateSeatHoldGroupItemEndpoint()
            .CancelSeatHoldGroupItemEndpoint()
            .confirmSeatHoldGroupItemEndpoint().RequireAuthorization();
    }
}