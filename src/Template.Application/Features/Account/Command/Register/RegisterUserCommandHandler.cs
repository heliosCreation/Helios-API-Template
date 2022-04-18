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
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApiResponse<RegisterUserResponse>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;


        public RegisterUserCommandHandler(
            IAuthenticationService authenticationService,
            IEmailService emailService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }
        public async Task<ApiResponse<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<RegisterUserResponse>();
            var result = await _authenticationService.RegisterAsync(request);
            if (!result.Succeeded)
            {
                return response.SetBadRequestResponse(null, result.Errors.ToList());
            }
            response.Data = new RegisterUserResponse { UserId = result.UserId };
            return response;
        }
    }
}
