#region

using CinemaTicketingSystem.SharedKernel.Identities;

#endregion

namespace CinemaTicketingSystem.Application.Abstraction.Accounts;

public interface ITokenService
{
    CreateTokenResponse CreateToken(CreateTokenRequest createToken);
}