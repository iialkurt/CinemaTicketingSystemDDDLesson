namespace CinemaTicketingSystem.Application.Abstraction.Ticketing;

public interface ISeatHoldAppService
{
    Task<AppResult> CreateSeatHoldAsync(CreateSeatHoldRequest request);
}