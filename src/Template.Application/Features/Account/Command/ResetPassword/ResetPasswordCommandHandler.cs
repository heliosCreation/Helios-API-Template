using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contracts.Identity;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ApiResponse<object>>
    {
        private readonly IAuthenticationService _authenticationService;

        public ResetPasswordCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }
        public async Task<ApiResponse<object>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _authenticationService.ResetPassword(request);
        }
    }
}
