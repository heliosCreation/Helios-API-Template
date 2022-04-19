using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.RegistrationToken
{
    public class RegistrationTokenCommand : IRequest<ApiResponse<RegistrationTokenResponse>>
    {
        public RegistrationTokenCommand()
        {

        }
        public RegistrationTokenCommand(string uid)
        {
            Uid = uid;
        }
        public string Uid { get; set; }
    }
}
