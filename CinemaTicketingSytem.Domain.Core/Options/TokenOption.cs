namespace CinemaTicketingSystem.SharedKernel.Options;

public class TokenOption
{
    public required string Issuer { get; set; }

    public required int AccessTokenExpiration { get; set; }


    public required int RefreshTokenExpiration { get; set; }
    public required string SecurityKey { get; set; }
}