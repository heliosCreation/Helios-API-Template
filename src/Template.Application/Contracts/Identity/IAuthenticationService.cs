using System.Threading.Tasks;
using Template.Application.Features.Account;
using Template.Application.Features.Account.Command.Authenticate;
using Template.Application.Features.Account.Command.Register;
using Template.Application.Model.Account;
using Template.Application.Models.Account.RefreshToken;
using Template.Application.Responses;

namespace Template.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticateCommand request);
        Task<ApiResponse<AuthenticationResponse>> RefreshTokenAsync(RefreshTokenRequest request);

        Task<ApiResponse<object>> ConfirmEmail(string email, string token);
        Task<string> GenerateRegistrationEncodedToken(string id);
        //Task<ApiResponse<RegistrationResponse>> RegisterAsync(RegistrationModel request);
        Task<CustomIdentityResult> RegisterAsync(RegisterUserCommand command);

        Task<bool> UserEmailExist(string email);
        Task<bool> UsernameExist(string name);
    }
}
