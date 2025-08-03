namespace CinemaTicketingSystem.Application.Abstraction.Accounts;

public interface IAccountAppService
{
    Task<AppResult<SignInResponse>> SignInAsync(SignInRequest userId);
    Task<AppResult> SignUpAsync(SignUpRequest request);
}