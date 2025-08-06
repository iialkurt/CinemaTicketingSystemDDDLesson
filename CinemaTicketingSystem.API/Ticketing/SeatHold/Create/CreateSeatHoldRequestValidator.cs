#region

using CinemaTicketingSystem.Application.Abstraction.Ticketing;
using FluentValidation;

#endregion

namespace CinemaTicketingSystem.API.Ticketing.SeatHold.Create;

public class CreateSeatHoldRequestValidator : AbstractValidator<CreateSeatHoldRequest>
{
    public CreateSeatHoldRequestValidator()
    {
        RuleFor(x => x.SeatPosition).NotEmpty();
        RuleFor(x => x.ScheduledMovieShowId).NotEmpty();
    }
}