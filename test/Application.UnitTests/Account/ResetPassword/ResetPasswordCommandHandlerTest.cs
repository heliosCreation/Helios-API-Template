using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Features.Account.Command.ResetPassword;
using Template.Application.Responses;
using Xunit;

namespace Application.UnitTests.Account.ResetPassword
{
    public class ResetPasswordCommandHandlerTest : AccountBaseTest
    {
        private readonly ResetPasswordCommandHandler _handler;
        private readonly ResetPasswordCommandValidator _validator;
        private readonly RequestHandlerHelper<ResetPasswordCommand, ResetPasswordCommandHandler, ResetPasswordCommandValidator, ApiResponse<object>> _helper;

        public ResetPasswordCommandHandlerTest()
        {
            _handler = new ResetPasswordCommandHandler(_mockAuthenticationService.Object);
            _validator = new ResetPasswordCommandValidator();
            _helper = new RequestHandlerHelper<ResetPasswordCommand, ResetPasswordCommandHandler, ResetPasswordCommandValidator, ApiResponse<object>>();
        }

        [Fact]
        public async Task Handle_WhenInputIsValid_ReturnsSuccess_AndData()
        {
            var command = new ResetPasswordCommand("test","test","test","test") { };
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.ShouldBeOfType<ApiResponse<object>>();
            result.Succeeded.ShouldBe(true);
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task Handle_WhenFailed_ReturnBadRequestResponse_AndErrors()
        {
            var command = new ResetPasswordCommand("testError", "test", "test", "test") { };
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.ShouldBeOfType<ApiResponse<object>>();
            result.Succeeded.ShouldBe(false);
            result.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            result.ErrorMessages.ShouldNotBeEmpty();
        }

    }
}
