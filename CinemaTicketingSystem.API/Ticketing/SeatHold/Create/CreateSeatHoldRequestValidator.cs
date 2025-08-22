#region

using CinemaTicketingSystem.Application.Contracts.Ticketing;
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