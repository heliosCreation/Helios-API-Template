using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contracts.Infrastructure;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.SendForgotPasswordMail
{
    public class SendForgotPasswordMailCommandHandler : IRequestHandler<SendForgotPasswordMailCommand, ApiResponse<string>>
    {
        private readonly IEmailService _emailService;

        public SendForgotPasswordMailCommandHandler(IEmailService emailService)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }
        public async Task<ApiResponse<string>> Handle(SendForgotPasswordMailCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<string>();
            var emailSent = await _emailService.SendForgotPasswordMail(request.Email, request.CallbackLink);

            if (!emailSent)
            {
                return response.SetInternalServerErrorResponse($"There was a problem trying to send the password reset mail for email {request.Email}.");
            }

            response.Data = request.CallbackLink;
            return response;
        }
    }
}
