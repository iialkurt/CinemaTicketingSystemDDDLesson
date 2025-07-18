namespace CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema
{
    public record RemoveCinemaHallRequest(Guid CinemaId, Guid HallId)
    {
    }
}
