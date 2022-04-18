using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contracts.Identity;
using Template.Application.Features.Account.Command.Register;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.RegistrationToken
{
    public class RegistrationTokenCommandHandller : IRequestHandler<RegistrationTokenCommand, ApiResponse<RegistrationTokenResponse>>
    {
        private readonly IAuthenticationService _authenticationService;

        public RegistrationTokenCommandHandller(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }
        public async Task<ApiResponse<RegistrationTokenResponse>> Handle(RegistrationTokenCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<RegistrationTokenResponse>();
            var result = await _authenticationService.GenerateRegistrationEncodedToken(request.Uid);
            if (!result.IsSuccess)
            {
                return response.setNotFoundResponse(message: result.Error);
            }

            response.Data = result;
            return response;
        }
    }
}
