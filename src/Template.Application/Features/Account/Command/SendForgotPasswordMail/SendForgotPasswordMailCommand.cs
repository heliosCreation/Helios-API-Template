using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.SendForgotPasswordMail
{
    public class SendForgotPasswordMailCommand : IRequest<ApiResponse<object>>
    {
        public SendForgotPasswordMailCommand(string email, string callbackLink)
        {
            Email = email;
            CallbackLink = callbackLink;
        }
        public string Email { get; set; }
        public string CallbackLink { get; set; }
    }
}
