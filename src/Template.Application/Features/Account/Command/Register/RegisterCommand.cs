using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.Register
{
    public class RegisterCommand : IRequest<ApiResponse<RegistrationResponse>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmationPassword { get; set; }
    }
}
