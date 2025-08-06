#region

using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Accounts;

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Accounts;

public interface IAccountAppService
{
    Task<AppResult<SignInResponse>> SignInAsync(SignInRequest userId);
    Task<AppResult> SignUpAsync(SignUpRequest request);
}