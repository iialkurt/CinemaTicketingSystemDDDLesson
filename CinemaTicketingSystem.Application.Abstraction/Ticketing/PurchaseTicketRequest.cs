namespace CinemaTicketingSystem.Application.Abstraction.Ticketing;

public record PurchaseTicketRequest(string Row, int SeatNumber, Guid ScheduleId);
