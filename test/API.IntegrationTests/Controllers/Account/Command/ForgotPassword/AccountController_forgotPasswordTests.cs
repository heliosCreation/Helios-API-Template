using Api.IntegrationTest.Base;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Template.API.Contract;
using Template.Application.Features.Account.Command.ForgotPassword;
using Template.Application.Features.Account.Command.Register;
using Template.Application.Responses;
using Xunit;

namespace API.IntegrationTests.Controllers.Account.Command.ForgotPassword
{
    using static ApiRoutes.Account;

    public class AccountController_forgotPasswordTests : IntegrationTestBase
    {
        [Fact]
        public async Task ForgotPassword_willReturns_CorrectStatusCodeAndData_WhenValidEmailIsPassed()
        {
            var response = await TestClient.PostAsJsonAsync(ForgotPwd, new ForgotPasswordCommand("john@gmail.com"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ForgotPassword_willNotReturn_OkStatusCode_whenInvalidEmailIsPassed()
        {
            var response = await TestClient.PostAsJsonAsync(ForgotPwd, new ForgotPasswordCommand("test@gmail.com"));
            response.StatusCode.Should().NotBe(HttpStatusCode.OK);
        }
    }
}
