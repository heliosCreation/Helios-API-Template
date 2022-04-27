using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Template.Application.Features.Account.Command.SendRegistrationMail;
using Template.Application.Responses;
using Xunit;

namespace Application.UnitTests.Account.SendRegistrationMail
{
    public class SendRegistrationMailCommandHandlerUnitTest : AccountBaseTest
    {
        private readonly SendRegistrationMailCommandHandler _handler;
        private readonly SendRegistrationMailCommandValidator _validator;
        private readonly RequestHandlerHelper<SendRegistrationMailCommand, SendRegistrationMailCommandHandler, SendRegistrationMailCommandValidator, ApiResponse<object>> _helper;


        public SendRegistrationMailCommandHandlerUnitTest()
        {
            _handler = new SendRegistrationMailCommandHandler(_mockEmailService.Object);
            _validator = new SendRegistrationMailCommandValidator();
            _helper = new RequestHandlerHelper<SendRegistrationMailCommand, SendRegistrationMailCommandHandler, SendRegistrationMailCommandValidator, ApiResponse<object>>();
        }

        [Fact]
        public async Task Handle_WhenValid_ReturnsOkStatus_AndCallbackLink()
        {
            var command = new SendRegistrationMailCommand("test@test.com", "callback");
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.ShouldBeOfType<ApiResponse<object>>();
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Data.ShouldNotBeNull();
        }

        [Fact]
        public async Task Handle_WhenInvalid_ReturnsInternalServerError_AndErrorMessage()
        {
            var command = new SendRegistrationMailCommand("test@Error.com", "callback");
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.ShouldBeOfType<ApiResponse<object>>();
            result.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
            result.ErrorMessages.ShouldNotBeEmpty();
            result.Data.ShouldBeNull();
        }


        [Theory]
        [InlineData("notanemail", "test")]
        [InlineData("", "test")]
        [InlineData("test@gmail.com", "")]
        [InlineData(null, "test")]
        [InlineData("test@gmail.com", null)]
        public async Task Handle_WhenValidatorsFail_ReturnsBadRequest_AndErrorMessage(string email, string callback)
        {
            var command = new SendRegistrationMailCommand(email, callback);
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.ShouldBeOfType<ApiResponse<object>>();
            result.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            result.ErrorMessages.ShouldNotBeEmpty();
            result.Data.ShouldBeNull();
        }

    }
}
