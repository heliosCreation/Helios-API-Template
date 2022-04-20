using Api.IntegrationTest.Base;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Template.API.Contract;
using Template.Application.Features.Account;
using Template.Application.Features.Account.Command.Authenticate;
using Template.Application.Features.Account.Command.RefreshToken;
using Template.Application.Responses;
using Xunit;

namespace API.IntegrationTests.Controllers.Account.Command.RefreshToken
{
    using static Api.IntegrationTests.Utils;
    using static ApiRoutes.Account;

    public class AccountController_refreshTokenTests : IntegrationTestBase
    {

        [Fact]
        public async Task RefreshToken_willReturns_BadRequest_WhenTokenIsNotExpired()
        {
            var loginResponse = await TestClient.PostAsJsonAsync(Authenticate,
            new AuthenticateCommand
            {
                Email = "john@gmail.com",
                Password = "Pwd12345!"
            });
            var content = await loginResponse.Content.ReadAsAsync<ApiResponse<AuthenticationResponse>>();
            IEnumerable<string> cookies = loginResponse.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

            var token = content.Data.Token;
            var refreshTokenCookie = cookies.FirstOrDefault(c => c.Contains("refreshToken"));
            var refreshToken = refreshTokenCookie.Split(";")[0].Replace("refreshToken=", "");

            var refreshTokenResponse = await TestClient.PostAsJsonAsync(Refresh,
                new ResfreshTokenCommand
                {
                    Token = token,
                    RefreshToken = refreshToken
                });

            var refreshTokenContent = await refreshTokenResponse.Content.ReadAsAsync<ApiResponse<object>>();

            refreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            refreshTokenContent.ErrorMessages[0].Should().Be("Token hasn't expired yet.");
        }

    }
}
