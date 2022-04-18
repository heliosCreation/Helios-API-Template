using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contracts.Identity;
using Template.Application.Contracts.Infrastructure;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApiResponse<RegistrationResponse>>
    {
        private readonly IAuthenticationService _authenticationService;

        public RegisterUserCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }
        public async Task<ApiResponse<RegistrationResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<RegistrationResponse>();
            var result = await _authenticationService.RegisterAsync(request);
            if (!result.Succeeded)
            {
                return response.SetBadRequestResponse(errors:result.Errors.ToList());
            }
            response.Data = new RegistrationResponse { UserId = result.UserId };
            return response;
        }
    }
}
