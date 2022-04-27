using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Template.Application.Features.Account;
using Template.Application.Features.Account.Command.RefreshToken;
using Template.Application.Responses;
using Xunit;

namespace Application.UnitTests.Account.RefreshToken
{
    public class RefreshTokenCommandaHandlerTest : AccountBaseTest
    {
        private readonly ResfreshTokenCommandHandler _handler;
        private readonly RefreshTokenCommandValidator _validator;
        private readonly RequestHandlerHelper<ResfreshTokenCommand, ResfreshTokenCommandHandler, RefreshTokenCommandValidator, ApiResponse<AuthenticationResponse>> _helper;

        public RefreshTokenCommandaHandlerTest()
        {
            _handler = new ResfreshTokenCommandHandler(_mockAuthenticationService.Object);
            _validator = new RefreshTokenCommandValidator();
            _helper = new RequestHandlerHelper<ResfreshTokenCommand, ResfreshTokenCommandHandler, RefreshTokenCommandValidator, ApiResponse<AuthenticationResponse>>();
        }

        [Fact]
        public async Task Handle_WhenInputIsValid_ReturnsSuccess_AndData()
        {
            var command = new ResfreshTokenCommand() { Token ="testToken",RefreshToken="testRefreshToken"};
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.Succeeded.ShouldBe(true);
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.ShouldBeOfType<ApiResponse<AuthenticationResponse>>();
            result.Data.Token.ShouldNotBeNullOrEmpty();
            result.Data.RefreshToken.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_WhenInputIsInvalid_ReturnsBadRequest_AndErrorMessage()
        {
            var command = new ResfreshTokenCommand() { Token = "testError", RefreshToken = "testRefreshToken" };
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.ShouldBeOfType<ApiResponse<AuthenticationResponse>>();
            result.Succeeded.ShouldBe(false);
            result.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            result.Data.ShouldBeNull();
            result.ErrorMessages.ShouldNotBeNull();
        }

    }
}
