using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contracts.Infrastructure;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.SendRegistrationMail
{
    public class SendRegistrationMailCommandHandler : IRequestHandler<SendRegistrationMailCommand, ApiResponse<object>>
    {
        private readonly IEmailService _emailService;

        public SendRegistrationMailCommandHandler(IEmailService emailService)
        {
            _emailService = emailService ?? throw new System.ArgumentNullException(nameof(emailService));
        }
        public async Task<ApiResponse<object>> Handle(SendRegistrationMailCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<object>();
            var mailSent = await _emailService.SendRegistrationMail(request.Email, request.CallBackUrl);
            if (!mailSent)
            {
                return response.SetInternalServerErrorResponse($"There was a problem trying to send the registration email for user with Email {request.Email}.");
            }

            return response;
        }
    }
}
