namespace CinemaTicketingSystem.Application.Catalog.ICL.Dto;

public record GetCatalogInfoResponse(string CinemaName, string HallName, string MovieTitle, short SeatCount);