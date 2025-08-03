namespace CinemaTicketingSystem.SharedKernel.Identities;

public record CreateTokenResponse(string AccessToken, string RefreshToken, DateTime AccessTokenExpiration, DateTime RefreshTokenExpiration);