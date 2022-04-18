using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Template.API.Contract;
using Template.Application.Contracts.Identity;
using Template.Application.Contracts.Infrastructure;
using Template.Application.Features.Account.Command.Authenticate;
using Template.Application.Features.Account.Command.ConfirmEmail;
using Template.Application.Features.Account.Command.RefreshToken;
using Template.Application.Features.Account.Command.Register;

namespace Template.API.Controllers
{
    using static ApiRoutes.Account;

    [AllowAnonymous]
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
        public async Task<IActionResult> AuthenticateAsync(AuthenticateCommand request)
        {
            var response = await Mediator.Send(request);
            if (response.Succeeded)
            {
                setTokenCookie(response.Data.RefreshToken);
            }
            return Ok(response);
        }


        [HttpPost(Refresh)]
        public async Task<IActionResult> RefreshTokenAsync(ResfreshTokenCommand request)
        {
            var response = await Mediator.Send(request);
            if (response.Succeeded)
            {
                setTokenCookie(response.Data.RefreshToken);
            }
            return Ok(response);
        }

        [HttpPost(Register)]
        public async Task<IActionResult> RegisterAsync(RegisterUserCommand request)
        {
            var response = await Mediator.Send(request);
            if (response.Succeeded)
            {
                var code = await _authenticationService.GenerateRegistrationEncodedToken(response.Data.UserId);
                var callbackLink = Url.ActionLink("ConfirmEmail", "Account", new { Email = request.Email, code = code });

                await _emailService.SendRegistrationMail(request.Email, callbackLink);
                response.Data.CallBackUrl = callbackLink;
            }
            return Ok(response);
        }
        [HttpGet(ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmailAsync(ConfirmEmailCommand request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }


        //Helper methods

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

    }
}
