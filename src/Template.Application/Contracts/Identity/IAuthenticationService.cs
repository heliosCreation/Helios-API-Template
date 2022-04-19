using System.Threading.Tasks;
using Template.Application.Features.Account;
using Template.Application.Features.Account.Command.Authenticate;
using Template.Application.Features.Account.Command.ConfirmEmail;
using Template.Application.Features.Account.Command.RefreshToken;
using Template.Application.Features.Account.Command.Register;
using Template.Application.Features.Account.Command.RegistrationToken;
using Template.Application.Features.Account.Command.ResetPassword;
using Template.Application.Responses;

namespace Template.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<RegistrationResponse> RegisterAsync(RegisterUserCommand command);
        Task<RegistrationTokenResponse> GenerateRegistrationEncodedToken(string id);
        Task<ApiResponse<object>> ConfirmEmail(ConfirmEmailCommand request);
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticateCommand request);
        Task<AuthenticationResponse> RefreshTokenAsync(ResfreshTokenCommand request);


        Task<bool> UserEmailExist(string email);
        Task<bool> UsernameExist(string name);
        Task<string> GetUserIdAsync(string email);
        Task<string> GeneratePasswordForgottenMailToken(string email);
        Task<ApiResponse<object>> ResetPassword(ResetPasswordCommand request);
    }
}
