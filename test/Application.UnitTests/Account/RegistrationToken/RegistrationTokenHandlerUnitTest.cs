using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Features.Account.Command.RegistrationToken;
using Template.Application.Responses;
using Xunit;

namespace Application.UnitTests.Account.RegistrationToken
{
    public class RegistrationTokenHandlerUnitTest : AccountBaseTest
    {
        private readonly RegistrationTokenCommandHandller _handler;
        private readonly RegistrationTokenCommandValidator _validator;
        private readonly RequestHandlerHelper<RegistrationTokenCommand, RegistrationTokenCommandHandller, RegistrationTokenCommandValidator, ApiResponse<RegistrationTokenResponse>> _helper;
        
        public RegistrationTokenHandlerUnitTest()
        {
            _handler = new RegistrationTokenCommandHandller(_mockAuthenticationService.Object);
            _validator = new RegistrationTokenCommandValidator();
            _helper = new RequestHandlerHelper<RegistrationTokenCommand, RegistrationTokenCommandHandller, RegistrationTokenCommandValidator, ApiResponse<RegistrationTokenResponse>>();
        }

        [Fact]
        public async Task Handle_WhenInputIsValid_ReturnsSuccess_AndData()
        {
            var command = new RegistrationTokenCommand() {Uid = "test" };
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.Succeeded.ShouldBe(true);
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.ShouldBeOfType<ApiResponse<RegistrationTokenResponse>>();
            result.Data.Token.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ReturnNotFound_AndError()
        {
            var command = new RegistrationTokenCommand() { Uid = "testError" };
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.ShouldBeOfType<ApiResponse<RegistrationTokenResponse>>();
            result.Succeeded.ShouldBe(false);
            result.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
            result.ErrorMessages.ShouldNotBeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task Handle_WhenInputInvalid_ReturnBadRequest_AndError(string uid)
        {
            var command = new RegistrationTokenCommand() { Uid = uid };
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.ShouldBeOfType<ApiResponse<RegistrationTokenResponse>>();
            result.Succeeded.ShouldBe(false);
            result.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            result.ErrorMessages.ShouldNotBeEmpty();
        }
    }
}
