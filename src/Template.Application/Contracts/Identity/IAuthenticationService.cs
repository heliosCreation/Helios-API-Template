using System.Threading.Tasks;
using Template.Application.Features.Account.Command;
using Template.Application.Model.Account;
using Template.Application.Model.Account.Authentification;
using Template.Application.Responses;

namespace Template.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
        Task<ApiResponse<object>> ConfirmEmail(string email, string token);
        Task<string> GenerateRegistrationEncodedToken(string id);
        //Task<ApiResponse<RegistrationResponse>> RegisterAsync(RegistrationModel request);
        Task<CustomIdentityResult> RegisterAsync(RegisterUserCommand command);

        Task<bool> UserEmailExist(string email);
        Task<bool> UsernameExist(string name);
    }
}
