using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<ApiResponse<ForgotPasswordResponse>>
    {
        public ForgotPasswordCommand(string email)
        {
            Email = email;
        }
        public string Email { get; set; }
    }
}
