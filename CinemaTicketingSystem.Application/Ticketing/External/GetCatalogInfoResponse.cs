namespace CinemaTicketingSystem.Application.Ticketing.External;

public record GetCatalogInfoResponse(string CinemaName, string HallName, string MovieTitle, short SeatCount);