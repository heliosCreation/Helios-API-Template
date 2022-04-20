using Api.IntegrationTest.Base;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Template.API.Contract;
using Template.Application.Features.Account;
using Template.Application.Features.Account.Command.Authenticate;
using Template.Application.Features.Account.Command.ConfirmEmail;
using Template.Application.Features.Account.Command.Register;
using Template.Application.Responses;
using Xunit;

namespace Api.IntegrationTest.Controllers.Account.Command
{
    using static Api.IntegrationTests.Utils.AccountTools;
    using static ApiRoutes.Account;


    public class AccountController_AccountTests : IntegrationTestBase
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

        #region Register
        [Fact]
        public async Task Register_willReturns_CorrectStatusCodeAndData_WhenValidCredentialsArePassed()
        {
            var registrationResponse = await TestClient.PostAsJsonAsync(Register,
                new RegisterUserCommand
                {
                    Email = "test@gmail.com",
                    Password = "Pwd12345!",
                    ConfirmationPassword = "Pwd12345!",
                    FirstName = "test",
                    LastName = "test",
                    UserName = "test"
                });

            var registrationContent = await registrationResponse.Content.ReadAsAsync<ApiResponse<RegistrationResponse>>();

            registrationResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            registrationContent.Data.Should().NotBeNull();
            registrationContent.Data.UserId.Should().NotBeNull();
        }

        [Theory]
        [ClassData(typeof(RegisterValidationTest))]
        public async Task Register_willReturns_BadRequest_WhenInValidFieldsArePassed(string firstname, string lastname, string username, string email, string password)
        {

            var response = await TestClient.PostAsJsonAsync(Register,
                new RegisterUserCommand
                {
                    Email = email,
                    Password = password,
                    FirstName = firstname,
                    LastName = lastname,
                    UserName = username
                });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }
        #endregion

        #region EmailConfirmation
        [Fact]
        public async Task EmailConfirmation_returnsOKStatusCode()
        {
            var registrationResponse = await TestClient.PostAsJsonAsync(Register,
                new RegisterUserCommand
                {
                    Email = "test@gmail.com",
                    Password = "Pwd12345!",
                    ConfirmationPassword = "Pwd12345!",
                    FirstName = "test",
                    LastName = "test",
                    UserName = "test"
                });

            var registrationContent = await registrationResponse.Content.ReadAsAsync<ApiResponse<RegistrationResponse>>();
            Uri myUri = new Uri(registrationContent.Data.CallBackUrl);
            string token = HttpUtility.ParseQueryString(myUri.Query).Get("token");

            var confirmationResponse = await TestClient.PostAsJsonAsync(ConfirmEmail,
                new ConfirmEmailCommand
                {
                    Email = "test@gmail.com",
                    RegistrationToken = token
                });

            confirmationResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task EmailConfirmation_enableUserLogin()
        {
            var registrationResponse = await TestClient.PostAsJsonAsync(Register,
                new RegisterUserCommand
                {
                    Email = "test@gmail.com",
                    Password = "Pwd12345!",
                    ConfirmationPassword ="Pwd12345!",
                    FirstName = "test",
                    LastName = "test",
                    UserName = "test"
                });

            var registrationContent = await registrationResponse.Content.ReadAsAsync<ApiResponse<RegistrationResponse>>();
            Uri myUri = new Uri(registrationContent.Data.CallBackUrl);
            string token = HttpUtility.ParseQueryString(myUri.Query).Get("token");

            var confirmationResponse = await TestClient.PostAsJsonAsync(ConfirmEmail,
                new ConfirmEmailCommand
                { 
                    Email= "test@gmail.com",
                    RegistrationToken = token
                });

            var authResponse = await TestClient.PostAsJsonAsync(Authenticate,
                new AuthenticateCommand
                {
                    Email = "test@gmail.com",
                    Password = "Pwd12345!"
                });

            authResponse.StatusCode.Should().Be(HttpStatusCode.OK);


        }
        #endregion
    }
}
