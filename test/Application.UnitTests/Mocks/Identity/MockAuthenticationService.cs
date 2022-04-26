using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Template.Application.Contracts.Identity;
using Template.Application.Features.Account;
using Template.Application.Features.Account.Command.Authenticate;
using Template.Application.Features.Account.Command.ConfirmEmail;
using Template.Application.Features.Account.Command.RefreshToken;
using Template.Application.Features.Account.Command.Register;
using Template.Application.Features.Account.Command.RegistrationToken;
using Template.Application.Features.Account.Command.ResetPassword;
using Template.Application.Responses;

namespace Application.UnitTests.Mocks.Identity
{
    public class MockAuthenticationService
    {

        public Mock<IAuthenticationService> MockService { get; set; } = new Mock<IAuthenticationService>();

        public Mock<IAuthenticationService> GetEntityRepository()
        {

            #region register
            //Registration
            MockService.Setup(s => s.RegisterAsync(It.IsAny<RegisterCommand>())).ReturnsAsync(new RegistrationResponse { CallBackUrl = "callbackTest", UserId = Guid.NewGuid().ToString() });
            MockService.Setup(s => s.RegisterAsync(It.Is<RegisterCommand>(c => c.FirstName == "Error"))).ReturnsAsync(new RegistrationResponse(new List<string> { "errors were made." }));

            //Token creation
            MockService.Setup(s => s.GenerateRegistrationEncodedToken(It.IsAny<string>())).ReturnsAsync(new RegistrationTokenResponse { IsSuccess = true, Token = "test" });
            MockService.Setup(s => s.GenerateRegistrationEncodedToken(It.Is<string>(s => s == "testError"))).ReturnsAsync(new RegistrationTokenResponse { IsSuccess = false, Error = "test error" });

            #endregion

            #region confirm email
            MockService.Setup(s => s.ConfirmEmail(It.IsAny<ConfirmEmailCommand>())).ReturnsAsync(new ApiResponse<object>());
            #endregion

            #region authenticate
            MockService.Setup(s => s.AuthenticateAsync(It.IsAny<AuthenticateCommand>())).ReturnsAsync((AuthenticateCommand request) =>
            {

                return new AuthenticationResponse
                {
                    Token = Guid.NewGuid().ToString(),
                    RefreshToken = Guid.NewGuid().ToString()
                };
            });

            MockService.Setup(s => s.AuthenticateAsync(It.Is<AuthenticateCommand>(c => c.Email == "wrong@wrong.com"))).ReturnsAsync((AuthenticateCommand request) =>
            {

                return new AuthenticationResponse
                {
                    IsSuccess = false
                };
            });
            #endregion

            #region refreshToken
            MockService.Setup(s => s.RefreshTokenAsync(It.IsAny<ResfreshTokenCommand>())).ReturnsAsync(new AuthenticationResponse { Token = "test", RefreshToken = "test" });
            MockService.Setup(s => s.RefreshTokenAsync(It.Is<ResfreshTokenCommand>(c => c.Token == "testError"))).ReturnsAsync(new AuthenticationResponse { IsSuccess = false, ErrorMessage = "This is an error." });
            #endregion

            #region forgotPassword
            MockService.Setup(s => s.GeneratePasswordForgottenMailToken(It.IsAny<string>())).ReturnsAsync(Guid.NewGuid().ToString());
            MockService.Setup(s => s.GeneratePasswordForgottenMailToken(It.Is<string>(s => s == "Error2@error.com"))).ReturnsAsync((string)null);
            #endregion

            #region resetPassword
            MockService.Setup(s => s.ResetPassword(It.IsAny<ResetPasswordCommand>())).ReturnsAsync(new ResetPasswordResponse { IsSuccess = true });
            MockService.Setup(s => s.ResetPassword(It.Is<ResetPasswordCommand>(c => c.Uid == "testError"))).ReturnsAsync(new ResetPasswordResponse { IsSuccess = false, ErrorMessage ="test error" });
            #endregion


            MockService.Setup(s => s.GetUserIdAsync(It.IsAny<string>())).ReturnsAsync(Guid.NewGuid().ToString());
            MockService.Setup(s => s.GetUserIdAsync(It.Is<string>(s => s == "Error@error.com"))).ReturnsAsync((string)null);
            #region private methods
            MockService.Setup(s => s.UsernameExist(It.IsAny<string>())).ReturnsAsync(true);
            MockService.Setup(s => s.UsernameExist(It.Is<string>(n => n == "Exist"))).ReturnsAsync(false);

            MockService.Setup(s => s.UserEmailExist(It.IsAny<string>())).ReturnsAsync(true);
            MockService.Setup(s => s.UserEmailExist(It.Is<string>(e => e == "Exist@test.com"))).ReturnsAsync(false);
            #endregion

            return MockService;
        }

        private JwtSecurityToken GenerateJwtSecurityToken()
        {
            var symmetricSecurityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(signingCredentials: signingCredentials);
        }
    }
}
