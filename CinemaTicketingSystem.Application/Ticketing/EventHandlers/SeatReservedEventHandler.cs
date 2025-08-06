#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations.DomainEvents;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using MediatR;

#endregion

namespace CinemaTicketingSystem.Application.Ticketing.EventHandlers;

public class SeatReservedEventHandler(ISeatHoldRepository seatHoldRepository) : INotificationHandler<SeatReservedEvent>
{
    public async Task Handle(SeatReservedEvent notification, CancellationToken cancellationToken)
    {
        var seatHoldToDelete = await seatHoldRepository.GetAsync(
            x => x.ScheduledMovieShowId == notification.ScheduledMovieShowId &&
                 x.CustomerId == notification.CustomerId &&
                 x.SeatPosition.Number == notification.SeatPosition.Number &&
                 x.SeatPosition.Row == notification.SeatPosition.Row, cancellationToken);

        if (seatHoldToDelete is null)
            throw new BusinessException(ErrorCodes.SeatHoldNotFound).AddData(notification.SeatPosition.Row)
                .AddData(notification.SeatPosition.Number);

        if (!seatHoldToDelete.CanBeConvertedToReservationOrPurchase())
            throw new BusinessException(ErrorCodes.SeatHoldExpired)
                .AddData(notification.SeatPosition.Row)
                .AddData(notification.SeatPosition.Number);

        await seatHoldRepository.DeleteAsync(seatHoldToDelete, cancellationToken);
    }
}