#region

using CinemaTicketingSystem.Application.Contracts.Ticketing;
using FluentValidation;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.TicketIssuance.Create;

public class CreateTicketIssuanceRequestValidator : AbstractValidator<CreateTicketIssuanceRequest>
{
    public CreateTicketIssuanceRequestValidator()
    {
        RuleFor(x => x.SeatPositionList).NotEmpty();
        RuleFor(x => x.ScheduledMovieShowId).NotEmpty();

        RuleFor(x => x.SeatPositionList).Must(x => x.Count > 0).WithMessage("Please select at least one seat.");
    }
}