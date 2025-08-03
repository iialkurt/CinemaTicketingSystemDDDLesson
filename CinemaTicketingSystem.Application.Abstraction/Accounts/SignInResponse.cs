namespace CinemaTicketingSystem.Application.Abstraction.Accounts
{
    public record SignInResponse(
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpiration,
        DateTime RefreshTokenExpiration);
}
