using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema;

namespace CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie.Hall
{
    public record CinemaHallDto(string Name, int[] SupportedTechnologies, List<SeatDto> Seats);
}
