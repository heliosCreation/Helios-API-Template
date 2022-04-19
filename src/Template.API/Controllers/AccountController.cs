using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Template.API.Contract;
using Template.Application.Features.Account.Command.Authenticate;
using Template.Application.Features.Account.Command.ConfirmEmail;
using Template.Application.Features.Account.Command.ForgotPassword;
using Template.Application.Features.Account.Command.RefreshToken;
using Template.Application.Features.Account.Command.Register;
using Template.Application.Features.Account.Command.RegistrationToken;
using Template.Application.Features.Account.Command.ResetPassword;
using Template.Application.Features.Account.Command.SendForgotPasswordMail;
using Template.Application.Features.Account.Command.SendRegistrationMail;

namespace Template.API.Controllers
{
    using static ApiRoutes.Account;

    [AllowAnonymous]
    public class AccountController : ApiController
    {
        public AccountController()
        {
        }

        [HttpPost(Register)]
        public async Task<IActionResult> RegisterAsync(RegisterUserCommand request)
        {
            var registrationResponse = await Mediator.Send(request);
            if (registrationResponse.Succeeded)
            {
                var tokenResponse = await Mediator.Send(new RegistrationTokenCommand(registrationResponse.Data.UserId));
                if (!tokenResponse.Succeeded)
                {
                    return Ok(registrationResponse.setNotFoundResponse(message: tokenResponse.ErrorMessages[0]));
                }
                var callbackLink = Url.ActionLink("ConfirmEmail", "Account", new { Email = request.Email, code = tokenResponse.Data.Token });

                var mailResponse = await Mediator.Send(new SendRegistrationMailCommand(request.Email, callbackLink));
                if (!mailResponse.Succeeded)
                {
                    registrationResponse.SetInternalServerErrorResponse(message: mailResponse.ErrorMessages[0]);
                    return Ok(registrationResponse);
                }
                registrationResponse.Data.CallBackUrl = callbackLink;
            }
            return Ok(registrationResponse);
        }

        [HttpPost(ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmailAsync(ConfirmEmailCommand request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
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

        [HttpPost(ForgotPwd)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand request)
        {
            var resetPasswordTokenResponse = await Mediator.Send(request);
            if (!resetPasswordTokenResponse.Succeeded)
            {
                return Ok(resetPasswordTokenResponse);
            }

            var callbackLink = Url.ActionLink("ResetPassword", "Account", new { uid = resetPasswordTokenResponse.Data.UserId, token = resetPasswordTokenResponse.Data.ResetPasswordToken });
            var resetPasswordMailResponse = await Mediator.Send(new SendForgotPasswordMailCommand(request.Email, callbackLink));

            return Ok(resetPasswordMailResponse);
        }

        [HttpPost(ResetPwd)]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand model)
        {
            return Ok();
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
