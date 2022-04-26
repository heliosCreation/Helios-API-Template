using Shouldly;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Template.Application;
using Template.Application.Features.Account.Command.Register;
using Template.Application.Responses;
using Xunit;
using static UnitTests.Utils.IdentitySet.AccountSet;

namespace Application.UnitTests.Account.Register
{
    public class RegisterCommandHandlerTest : AccountBaseTest
    {
        private readonly RegisterCommandHandler _handler;
        private readonly RegisterCommandValidator _validator;
        private readonly RequestHandlerHelper<RegisterCommand, RegisterCommandHandler, RegisterCommandValidator, ApiResponse<RegistrationResponse>> _helper;

        public RegisterCommandHandlerTest()
        {
            _handler = new RegisterCommandHandler(_mockAuthenticationService.Object);
            _validator = new RegisterCommandValidator(_mockAuthenticationService.Object);
            _helper = new RequestHandlerHelper<RegisterCommand, RegisterCommandHandler, RegisterCommandValidator, ApiResponse<RegistrationResponse>>();
        }

        [Fact]
        public async Task Handle_WhenInputIsValid_ReturnsSuccess_AndData()
        {
            var command = new RegisterCommand() { Email = "Test@test.com", Password = "Pwd12345!", ConfirmationPassword = "Pwd12345!", FirstName = "John", LastName = "Doe", UserName = "JohnDoe" };
            var result = await _helper.HandleRequest(command, _handler, _validator);


            result.Data.Succeeded.ShouldBe(true);
            result.Data.UserId.ShouldNotBeEmpty();
            result.Data.CallBackUrl.ShouldBeNull();
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        }

        [Theory]
        [ClassData(typeof(Register_BadRequest_Command))]
        public async Task Handle_RegisterCommand_WhenInvalid_ReturnsBadRequest_AndErrors(
            string email, string password, string confirmationPassword, string firstName, string lastName, string userName)
        {
            var command = new RegisterCommand() { Email = email, Password = password, ConfirmationPassword = confirmationPassword, FirstName = firstName, LastName = lastName, UserName = userName };
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            result.ErrorMessages.ShouldNotBeNull();
        }
    }
}
