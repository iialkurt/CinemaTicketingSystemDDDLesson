#region

using Asp.Versioning.Builder;
using CinemaTicketingSystem.Presentation.API.Purchase.Create;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Purchase;

public static class PurchaseEndpointExt
{
    public static void AddPurchaseGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/purchases").WithTags("purchases")
            .WithApiVersionSet(apiVersionSet)
            .CreatePurchaseGroupItemEndpoint();
    }
}