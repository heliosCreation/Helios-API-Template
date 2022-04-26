using Application.UnitTests.Mocks.Identity;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Template.Application;
using Template.Application.Contracts.Identity;
using Template.Application.Features.Account.Command.ConfirmEmail;
using Template.Application.Responses;
using Xunit;
using static UnitTests.Utils.IdentitySet.AccountSet;

namespace Application.UnitTests.Account.ConfirmEmail
{
    public class ConfirmEmailCommandHandlerTest : AccountBaseTest
    {
        private readonly ConfirmEmailCommandHandler _handler;
        private readonly ConfirmEmailCommandValidator _validator;
        private readonly RequestHandlerHelper<ConfirmEmailCommand, ConfirmEmailCommandHandler, ConfirmEmailCommandValidator, ApiResponse<object>> _helper;


        public ConfirmEmailCommandHandlerTest()
        {
            _handler = new ConfirmEmailCommandHandler(_mockAuthenticationService.Object);
            _validator = new ConfirmEmailCommandValidator();
            _helper = new RequestHandlerHelper<ConfirmEmailCommand, ConfirmEmailCommandHandler, ConfirmEmailCommandValidator, ApiResponse<object>>();
        }

        [Fact]
        public async Task Handle_WhenInputIsValid_ReturnsSuccess_AndData()
        {
            var command = new ConfirmEmailCommand { Email = "test@test.com", RegistrationToken = "token" };
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.Succeeded.ShouldBe(true);
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        }

        [Theory]
        [ClassData(typeof(ConfirmEmail_BadRequest_Command))]
        public async Task Handle_WhenInputAreInValid_ReturnsBadRequest_AndErrorMsg(string email, string token)
        {
            var command = new ConfirmEmailCommand { Email = email, RegistrationToken = token };
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.Succeeded.ShouldBe(false);
            result.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
        }

    }
}
