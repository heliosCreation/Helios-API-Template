using Api.IntegrationTest.Base;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Template.API.Contract;
using Template.Application.Features.Account;
using Template.Application.Features.Account.Command.Authenticate;
using Template.Application.Features.Account.Command.Register;
using Template.Application.Responses;
using Xunit;

namespace API.IntegrationTests.Controllers.Account.Command.Authenticate
{
    using static ApiRoutes.Account;

    public class AccountController_registerTests : IntegrationTestBase
    {
        #region Login
        [Fact]
        public async Task Login_willReturns_CorrectStatusCodeAndToken_WhenValidCredentialsArePassed()
        {

            var response = await TestClient.PostAsJsonAsync(Authenticate,
                new AuthenticateCommand
                {
                    Email = "john@gmail.com",
                    Password = "Pwd12345!"
                });
            var content = await response.Content.ReadAsAsync<ApiResponse<AuthenticationResponse>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Data.Should().NotBeNull();
            content.Data.Token.Should().NotBeNull();
        }

        [Fact]
        public async Task Login_willReturns_401StatusCodeAndNoData_WhenUnregisteredCredentialsArePassed()
        {

            var response = await TestClient.PostAsJsonAsync(Authenticate,
                new AuthenticateCommand
                {
                    Email = "unknown@gmail.com",
                    Password = "Pwd12345!"
                });
            var content = await response.Content.ReadAsAsync<ApiResponse<AuthenticationResponse>>();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            content.Data.Should().BeNull();
        }

        [Fact]
        public async Task Login_willFail_ifEmailIsNotConfirmed()
        {
            await TestClient.PostAsJsonAsync(Register,
                new RegisterUserCommand
                {
                    Email = "test@gmail.com",
                    Password = "Pwd12345!",
                    FirstName = "test",
                    LastName = "test",
                    UserName = "test"
                });

            var response = await TestClient.PostAsJsonAsync(Authenticate,
                new AuthenticateCommand
                {
                    Email = "test@gmail.com",
                    Password = "Pwd12345!"
                });
            var content = await response.Content.ReadAsAsync<ApiResponse<AuthenticationResponse>>();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            content.Data.Should().BeNull();
        }
        #endregion

    }
}
