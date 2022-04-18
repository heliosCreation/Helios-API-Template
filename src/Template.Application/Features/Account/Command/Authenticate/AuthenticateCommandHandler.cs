using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contracts.Identity;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.Authenticate
{
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, ApiResponse<AuthenticationResponse>>
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }
        public async Task<ApiResponse<AuthenticationResponse>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<AuthenticationResponse>();
            var result = await _authenticationService.AuthenticateAsync(request);
            if (!result.IsSuccess)
            {
                return response.SetUnhautorizedResponse();
            }

            response.Data = result;
            return response;
        }
    }
}
