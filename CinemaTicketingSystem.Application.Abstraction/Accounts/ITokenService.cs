using CinemaTicketingSystem.SharedKernel.Identities;

namespace CinemaTicketingSystem.Application.Abstraction.Accounts
{
    public interface ITokenService
    {

        CreateTokenResponse CreateToken(CreateTokenRequest createToken);
    }
}
