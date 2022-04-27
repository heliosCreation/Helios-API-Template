using Application.UnitTests.Mocks.Identity;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Template.Application;
using Template.Application.Contracts.Identity;
using Template.Application.Features.Account;
using Template.Application.Features.Account.Command.Authenticate;
using Template.Application.Responses;
using Xunit;
using static UnitTests.Utils.IdentitySet.AccountSet;

namespace Application.UnitTests.Account.Authenticate
{
    public class AuthenticateHandlerTests : AccountBaseTest
    {
        private readonly AuthenticateCommandHandler _handler;
        private readonly AuthenticateCommandValidator _validator;
        private readonly RequestHandlerHelper<AuthenticateCommand, AuthenticateCommandHandler, AuthenticateCommandValidator, ApiResponse<AuthenticationResponse>> _helper;
        public AuthenticateHandlerTests()
        {
            _handler = new AuthenticateCommandHandler(_mockAuthenticationService.Object);
            _validator = new AuthenticateCommandValidator();
            _helper = new RequestHandlerHelper<AuthenticateCommand, AuthenticateCommandHandler, AuthenticateCommandValidator, ApiResponse<AuthenticationResponse>>();
        }

        [Fact]
        public async Task Handle_WhenInputIsValid_ReturnsSuccess_AndData()
        {
            var command = new AuthenticateCommand() { Email = "Test@test.com", Password="Pwd12345!" };
            var result = await _helper.HandleRequest(command, _handler, _validator);


            result.Data.IsSuccess.ShouldBe(true);
            result.Data.Token.ShouldNotBeEmpty();
            result.Data.RefreshToken.ShouldNotBeEmpty();
        }
        [Theory]
        [ClassData(typeof(Authenticate_BadRequest_Command))]
        public async Task Handle_CategoryWhenInValid_IsNotAddedToRepo_AndContainsErrorsInResponse(string email, string password)
        {
            var command = new AuthenticateCommand() { Email = email, Password = password };
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            result.ErrorMessages.ShouldNotBeNull();
        }

        [Fact]
        public async Task Handle_WithNoSuccess_returns_unauthorizedStatusCode()
        {
            var command = new AuthenticateCommand() { Email = "wrong@wrong.com", Password = "Pwd12345!" };
            var result = await _helper.HandleRequest(command, _handler, _validator);


            result.StatusCode.ShouldBe((int)HttpStatusCode.Unauthorized);
            result.ErrorMessages.ShouldNotBeNull();
        }

    }
}
