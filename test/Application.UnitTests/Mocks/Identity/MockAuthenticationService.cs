using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Template.Application.Contracts.Identity;
using Template.Application.Features.Account;
using Template.Application.Features.Account.Command.Authenticate;
using Template.Application.Features.Account.Command.Register;
using Template.Identity.Entities;

namespace Application.UnitTests.Mocks.Identity
{
    public class MockAuthenticationService
    {

        public Mock<IAuthenticationService> MockService { get; set; } = new Mock<IAuthenticationService>();

        public Mock<IAuthenticationService> GetEntityRepository()
        {
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

            #region register
            MockService.Setup(s => s.RegisterAsync(It.IsAny<RegisterCommand>())).ReturnsAsync(new RegistrationResponse { CallBackUrl = "callbackTest", UserId = Guid.NewGuid().ToString() });
            MockService.Setup(s => s.RegisterAsync(It.Is<RegisterCommand>(c => c.FirstName == "Error"))).ReturnsAsync(new RegistrationResponse(new List<string> { "errors were made." }));
            #endregion


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
