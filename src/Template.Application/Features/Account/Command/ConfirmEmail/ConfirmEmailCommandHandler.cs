using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contracts.Identity;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ApiResponse<object>>
    {
        private readonly IAuthenticationService _authenticationService;

        public ConfirmEmailCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }
        public async Task<ApiResponse<object>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return await _authenticationService.ConfirmEmail(request);
        }
    }
}
