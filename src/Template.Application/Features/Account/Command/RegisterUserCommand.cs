using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command
{
    public class RegisterUserCommand : IRequest<ApiResponse<RegisterUserResponse>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
