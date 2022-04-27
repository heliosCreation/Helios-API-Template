using Api.IntegrationTest.Base;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Template.API.Contract;
using Template.Application.Features.Account.Command.Authenticate;
using Template.Application.Features.Account.Command.ConfirmEmail;
using Template.Application.Features.Account.Command.Register;
using Template.Application.Responses;
using Xunit;

namespace API.IntegrationTests.Controllers.Account.Command.ConfirmationEmail
{
    using static ApiRoutes.Account;

    public class AccountController_emailConfirmationTests : IntegrationTestBase
    {
        #region EmailConfirmation
        [Fact]
        public async Task EmailConfirmation_returnsOKStatusCode()
        {
            var registrationResponse = await TestClient.PostAsJsonAsync(Register,
                new RegisterCommand
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
                new RegisterCommand
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
