using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<ApiResponse<object>>
    {
        public string Email { get; set; }
        public string RegistrationToken { get; set; }
    }
}
