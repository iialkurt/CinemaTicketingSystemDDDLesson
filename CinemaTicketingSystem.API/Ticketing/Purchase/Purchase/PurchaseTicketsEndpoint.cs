#region

using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.API.Filters;
using CinemaTicketingSystem.Application.Abstraction.Ticketing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.API.Ticketing.Purchase.Purchase;

public static class PurchaseTicketsEndpoint
{
    public static RouteGroupBuilder PurchaseTicketsGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/purchases",
                async (PurchaseTicketRequest request, [FromServices] ITicketPurchaseAppService purchaseAppService) =>
                (await purchaseAppService.Purchase(request)).ToGenericResult())
            .WithName("purchaseTickets")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<ReserveSeatsRequest>>();


        return group;
    }
}