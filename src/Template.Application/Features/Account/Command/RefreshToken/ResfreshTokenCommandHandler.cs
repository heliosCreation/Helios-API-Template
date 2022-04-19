using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contracts.Identity;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.RefreshToken
{
    public class ResfreshTokenCommandHandler : IRequestHandler<ResfreshTokenCommand, ApiResponse<AuthenticationResponse>>
    {
        private readonly IAuthenticationService _authenticationService;

        public ResfreshTokenCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }
        public async Task<ApiResponse<AuthenticationResponse>> Handle(ResfreshTokenCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<AuthenticationResponse>();
            var result = await _authenticationService.RefreshTokenAsync(request);
            if (!result.IsSuccess)
            {
                return response.SetBadRequestResponse(message:result.ErrorMessage);
            }
            response.Data = result;
            return response; 
        }
    }
}
