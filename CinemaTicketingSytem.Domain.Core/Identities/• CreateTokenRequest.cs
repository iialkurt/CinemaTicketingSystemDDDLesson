namespace CinemaTicketingSystem.SharedKernel.Identities;

public record CreateTokenRequest(Guid UserId, string UserName, string Email);