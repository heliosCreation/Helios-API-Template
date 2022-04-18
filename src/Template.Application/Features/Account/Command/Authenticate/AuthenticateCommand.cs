using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.Authenticate
{
    public class AuthenticateCommand : IRequest<ApiResponse<AuthenticationResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
