using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Template.Application.Features.Account.Command.SendForgotPasswordMail;
using Template.Application.Responses;
using Xunit;

namespace Application.UnitTests.Account.SendForgotPasswordMail
{
    public class SendForgotPasswordMailCommandHandlerUnitTest : AccountBaseTest
    {
        private readonly SendForgotPasswordMailCommandHandler _handler;
        private readonly SendForgotPasswordMailCommandaValidator _validator;
        private readonly RequestHandlerHelper<SendForgotPasswordMailCommand, SendForgotPasswordMailCommandHandler, SendForgotPasswordMailCommandaValidator, ApiResponse<string>> _helper;
        public SendForgotPasswordMailCommandHandlerUnitTest()
        {
            _handler = new SendForgotPasswordMailCommandHandler(_mockEmailService.Object);
            _validator = new SendForgotPasswordMailCommandaValidator();
            _helper = new RequestHandlerHelper<SendForgotPasswordMailCommand, SendForgotPasswordMailCommandHandler, SendForgotPasswordMailCommandaValidator, ApiResponse<string>>();
        }

        [Fact]
        public async Task Handle_WhenValid_ReturnsOkStatus_AndCallbackLink()
        {
            var command = new SendForgotPasswordMailCommand("test@test.com", "callback");
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.ShouldBeOfType<ApiResponse<string>>();
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Data.ShouldNotBeNull();
        }

        [Fact]
        public async Task Handle_WhenInvalid_ReturnsInternalServerError_AndErrorMessage()
        {
            var command = new SendForgotPasswordMailCommand("test@Error.com", "callback");
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.ShouldBeOfType<ApiResponse<string>>();
            result.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
            result.ErrorMessages.ShouldNotBeEmpty();
            result.Data.ShouldBeNull();
        }


        [Theory]
        [InlineData("notanemail", "test")]
        [InlineData("", "test")]
        [InlineData("test@gmail.com", "")]
        [InlineData(null,"test")]
        [InlineData("test@gmail.com",null)]
        public async Task Handle_WhenValidatorsFail_ReturnsBadRequest_AndErrorMessage(string email, string callback)
        {
            var command = new SendForgotPasswordMailCommand(email, callback);
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.ShouldBeOfType<ApiResponse<string>>();
            result.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            result.ErrorMessages.ShouldNotBeEmpty();
            result.Data.ShouldBeNull();
        }

    }
}
