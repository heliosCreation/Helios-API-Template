using Api.IntegrationTest.Base;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Template.API.Contract;
using Template.Application.Features.Account.Command.ForgotPassword;
using Template.Application.Features.Account.Command.ResetPassword;
using Template.Application.Responses;
using Xunit;

namespace API.IntegrationTests.Controllers.Account.Command.ResetPassword
{
    using static ApiRoutes.Account;

    public class AccountController_resetPasswordTests : IntegrationTestBase
    {
        [Fact]
        public async Task ResetPassword_willReturns_CorrectStatusCode_WhenValidDataArePassed()
        {
            var forgotPwdResponse = await TestClient.PostAsJsonAsync(ForgotPwd, new ForgotPasswordCommand("john@gmail.com"));
            var forgotPwdContent = await forgotPwdResponse.Content.ReadAsAsync<ApiResponse<string>>();

            Uri myUri = new Uri(forgotPwdContent.Data);
            string uid = HttpUtility.ParseQueryString(myUri.Query).Get("uid");
            string token = HttpUtility.ParseQueryString(myUri.Query).Get("token");

            var resetPwdResponse = await TestClient.PostAsJsonAsync(ResetPwd, new ResetPasswordCommand
            {
                Uid = uid,
                ResetToken = token,
                NewPassword = "Pwd123456!",
                ConfirmationPassword = "Pwd123456!"
            });

            resetPwdResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact]
        public async Task ResetPassword_willReturns_InvalidStatusCode_WhenWrongTokenIsPassed()
        {
            var forgotPwdResponse = await TestClient.PostAsJsonAsync(ForgotPwd, new ForgotPasswordCommand("john@gmail.com"));
            var forgotPwdContent = await forgotPwdResponse.Content.ReadAsAsync<ApiResponse<string>>();

            Uri myUri = new Uri(forgotPwdContent.Data);
            string uid = HttpUtility.ParseQueryString(myUri.Query).Get("token");
            string token = HttpUtility.ParseQueryString(myUri.Query).Get("token");

            var resetPwdResponse = await TestClient.PostAsJsonAsync(ResetPwd, new ResetPasswordCommand
            {
                Uid = uid,
                ResetToken = token + "&",
                NewPassword = "Pwd123456!",
                ConfirmationPassword = "Pwd123456!"
            });

            resetPwdResponse.StatusCode.Should().NotBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ResetPassword_willReturns_InvalidStatusCode_WhenPasswordsDoNotMatch()
        {
            var forgotPwdResponse = await TestClient.PostAsJsonAsync(ForgotPwd, new ForgotPasswordCommand("john@gmail.com"));
            var forgotPwdContent = await forgotPwdResponse.Content.ReadAsAsync<ApiResponse<string>>();

            Uri myUri = new Uri(forgotPwdContent.Data);
            string uid = HttpUtility.ParseQueryString(myUri.Query).Get("token");
            string token = HttpUtility.ParseQueryString(myUri.Query).Get("token");

            var resetPwdResponse = await TestClient.PostAsJsonAsync(ResetPwd, new ResetPasswordCommand
            {
                Uid = uid,
                ResetToken = token + "&",
                NewPassword = "Pwd123456!",
                ConfirmationPassword = "NotMatchingPassword"
            });

            resetPwdResponse.StatusCode.Should().NotBe(HttpStatusCode.OK);
        }
    }
}
