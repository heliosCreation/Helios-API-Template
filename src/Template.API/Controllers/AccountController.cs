using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Template.API.Contract;
using Template.Application.Contracts.Identity;
using Template.Application.Contracts.Infrastructure;
using Template.Application.Features.Account.Command;
using Template.Application.Model.Account.Authentification;

namespace Template.API.Controllers
{
    using static ApiRoutes.Account;

    public class AccountController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;

        public AccountController(IAuthenticationService authenticationService, IEmailService emailService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        [HttpPost(Authenticate)]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var x = await _authenticationService.AuthenticateAsync(request);
            return Ok(x);
        }

        //[HttpPost(Register)]
        //public async Task<IActionResult> RegisterdddddAsync(RegistrationModel request)
        //{
        //    var response = await _authenticationService.RegisterAsync(request);
        //    if (response.Succeeded)
        //    {
        //        var code = await _authenticationService.GenerateRegistrationEncodedToken(response.Data.UserId);
        //        var callbackLink = Url.ActionLink("ConfirmEmail", "Account", new { Email = request.Email, code = code });

        //        await _emailService.SendRegistrationMail(request.Email, callbackLink);
        //        response.Data.callbackURL = callbackLink;
        //    }
        //    return Ok(response);
        //}



        [HttpPost(Register)]
        public async Task<IActionResult> RegisterAsync(RegisterUserCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.Succeeded)
            {
                var code = await _authenticationService.GenerateRegistrationEncodedToken(result.Data.UserId);
                var callbackLink = Url.ActionLink("ConfirmEmail", "Account", new { Email = command.Email, code = code });

                await _emailService.SendRegistrationMail(command.Email, callbackLink);
                result.Data.CallbackUrl = callbackLink;
            }
            return Ok(result);
        }
        [HttpGet(ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmailAsync(string email, string code)
        {
            var response = await _authenticationService.ConfirmEmail(email, code);
            return Ok(response);
        }
    }
}
