using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.RefreshToken
{
    public class ResfreshTokenCommand : IRequest<ApiResponse<AuthenticationResponse>>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
