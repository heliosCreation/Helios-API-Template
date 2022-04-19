using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.SendRegistrationMail
{
    public class SendRegistrationMailCommand : IRequest<ApiResponse<object>>
    {
        public SendRegistrationMailCommand()
        {

        }

        public SendRegistrationMailCommand(string email, string callBackUrl)
        {
            Email = email;
            CallBackUrl = callBackUrl;
        }
        public string Email { get; set; }
        public string CallBackUrl { get; set; }
    }
}
