using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contracts.Identity;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ApiResponse<ForgotPasswordResponse>>
    {
        private readonly IAuthenticationService _authenticationService;

        public ForgotPasswordCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }
        public async Task<ApiResponse<ForgotPasswordResponse>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<ForgotPasswordResponse>();
            var userId = await _authenticationService.GetUserIdAsync(request.Email);
            if (userId == null)
            {
                return response.setNotFoundResponse("No user was found with this associated email.");
            }

            var resetToken = await _authenticationService.GeneratePasswordForgottenMailToken(request.Email);
            if (resetToken == null)
            {
                return response.SetUnhautorizedResponse($"User with email address {request.Email} has not confirmed his/her email address.");
            }
            response.Data = new ForgotPasswordResponse(userId, resetToken);
            return response;
        }
    }
}
