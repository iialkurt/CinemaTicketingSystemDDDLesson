#region

using CinemaTicketingSystem.Application.Contracts.Ticketing;
using FluentValidation;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.Reservation.Reserve;

public class ReserveSeatsRequestValidator : AbstractValidator<CreateReservationRequest>
{
    public ReserveSeatsRequestValidator()
    {
        RuleFor(x => x.ScheduledMovieShowId).NotEmpty();
    }
}