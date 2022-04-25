using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contracts.Identity;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ApiResponse<RegistrationResponse>>
    {
        private readonly IAuthenticationService _authenticationService;

        public RegisterCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }
        public async Task<ApiResponse<RegistrationResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<RegistrationResponse>();
            var result = await _authenticationService.RegisterAsync(request);
            if (!result.Succeeded)
            {
                return response.SetBadRequestResponse(errors: result.Errors.ToList());
            }
            response.Data = new RegistrationResponse { UserId = result.UserId };
            return response;
        }
    }
}
