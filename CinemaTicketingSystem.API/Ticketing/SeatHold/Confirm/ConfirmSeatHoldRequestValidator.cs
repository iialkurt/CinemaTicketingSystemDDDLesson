#region

using CinemaTicketingSystem.Application.Contracts.Ticketing;
using FluentValidation;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.SeatHold.Confirm;

public class ConfirmSeatHoldRequestValidator : AbstractValidator<ConfirmSeatHoldRequest>
{
    public ConfirmSeatHoldRequestValidator()
    {
        RuleFor(x => x.ScheduledMovieShowId).NotEmpty();
    }
}