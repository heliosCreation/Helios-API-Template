using Api.IntegrationTest.Base;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Template.API.Contract;
using Template.Application.Features.Account.Command.Register;
using Template.Application.Responses;
using Xunit;

namespace API.IntegrationTests.Controllers.Account.Command.Register
{
    using static Api.IntegrationTests.Utils.AccountTools;
    using static ApiRoutes.Account;

    public class AccountController_registerTests : IntegrationTestBase
    {
        #region Register
        [Fact]
        public async Task Register_willReturns_CorrectStatusCodeAndData_WhenValidCredentialsArePassed()
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

            registrationResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            registrationContent.Data.Should().NotBeNull();
            registrationContent.Data.UserId.Should().NotBeNull();
        }

        [Theory]
        [ClassData(typeof(RegisterValidationTest))]
        public async Task Register_willReturns_BadRequest_WhenInValidFieldsArePassed(string firstname, string lastname, string username, string email, string password)
        {

            var response = await TestClient.PostAsJsonAsync(Register,
                new RegisterCommand
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

    }
}
