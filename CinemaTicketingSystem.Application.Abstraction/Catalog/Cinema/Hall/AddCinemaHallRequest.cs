namespace CinemaTicketingSystem.Application.Abstraction.Catalog.Cinema.Hall;

public record AddCinemaHallRequest(string Name, int[] Technologies, List<SeatPositionDto> SeatList)
{
}
