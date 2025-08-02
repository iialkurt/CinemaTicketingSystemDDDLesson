namespace CinemaTicketingSystem.SharedKernel.Identities;

public class CreateTokenResponse(string Token, string RefreshToken, DateTime AccessTokenExpiration, DateTime RefreshTokenExpiration);