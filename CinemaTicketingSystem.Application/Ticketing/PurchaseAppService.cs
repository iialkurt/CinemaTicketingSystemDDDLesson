using System.Net;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Application.Abstraction.Ticketing;
using CinemaTicketingSystem.Application.Catalog.ICL;
using CinemaTicketingSystem.Application.Schedules.ICL;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Repositories;
using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Ticketing;
using CinemaTicketingSystem.Domain.ValueObjects;
using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Application.Ticketing;

public class PurchaseAppService(
    AppDependencyService appDependencyService,
    IPurchaseRepository purchaseRepository,
    IUserContext userContext,
    ICatalogQueryService catalogQueryService,
    IScheduleQueryService iScheduleQueryService) : IScopedDependency, ITicketPurchaseAppService
{
    public async Task<AppResult> Purchase(PurchaseTicketRequest request)
    {
        var scheduleInfo = await iScheduleQueryService.GetScheduleInfo(request.ScheduleId);

        if (scheduleInfo.IsFail) return scheduleInfo;


        var catalogInfo =
            await catalogQueryService.GetCinemaInfo(scheduleInfo.Data!.CinemaHallId, scheduleInfo.Data.MovieId);


        var ticketPurchaseList = purchaseRepository.GetTicketsPurchaseByScheduleId(request.ScheduleId);


        var purchasedTicketCount = ticketPurchaseList.SelectMany(x => x.TicketList).Count();

        var availableSeatCount = catalogInfo.Data!.SeatCount - purchasedTicketCount;
        if (availableSeatCount <= 0)
            return appDependencyService.Error(ErrorCodes.SeatNotAvailable,
                HttpStatusCode.BadRequest);


        if (availableSeatCount < request.SeatPositionList.Count)
            return appDependencyService.Error(ErrorCodes.NotEnoughSeatsAvailable, [availableSeatCount],
                HttpStatusCode.BadRequest);


        foreach (var seat in request.SeatPositionList)
        {
            var seatNumber = new SeatPosition(seat.Row, seat.Number);
            var hasTicket = ticketPurchaseList.Any(x => x.HasTicketForSeat(seatNumber));
            if (hasTicket)
                return appDependencyService.Error(ErrorCodes.DuplicateSeat, [seat.Row, seat.Number],
                    HttpStatusCode.BadRequest);
        }


        var ticket = new Purchase(request.ScheduleId, userContext.UserId);

        foreach (var seat in request.SeatPositionList)
        {
            var newTicket = new Ticket(new SeatPosition(seat.Row, seat.Number), scheduleInfo.Data.TicketPrice);
            ticket.AddTicket(newTicket);
        }


        await purchaseRepository.AddAsync(ticket);

        await appDependencyService.UnitOfWork.SaveChangesAsync();

        return AppResult.SuccessAsNoContent();
    }
}
