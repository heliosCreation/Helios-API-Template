using Application.UnitTests.Mocks.Identity;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Template.Application;
using Template.Application.Contracts.Identity;
using Template.Application.Features.Account.Command.ForgotPassword;
using Template.Application.Responses;
using Xunit;
using static UnitTests.Utils.IdentitySet.AccountSet;

namespace Application.UnitTests.Account.ForgotPassword
{
    public class ForgotPasswordCommandHandlerTest
    {
        private readonly ForgotPasswordCommandHandler _handler;
        private readonly ForgotPasswordCommandValidator _validator;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;

        public ForgotPasswordCommandHandlerTest()
        {
            _mockAuthenticationService = new MockAuthenticationService().GetEntityRepository();

            _handler = new ForgotPasswordCommandHandler(_mockAuthenticationService.Object);
            _validator = new ForgotPasswordCommandValidator();
        }

        [Fact]
        public async Task Handle_WhenInputIsValid_ReturnsSuccess_AndData()
        {
            var command = new ForgotPasswordCommand(email: "test@test.com");
            var validationBehavior = new ValidationBehaviour<ForgotPasswordCommand, ApiResponse<ForgotPasswordResponse>>(new List<ForgotPasswordCommandValidator>()
            {
                _validator
            });
            var result = await validationBehavior.Handle(command, CancellationToken.None, () =>
            {
                return _handler.Handle(command, cancellationToken: CancellationToken.None);
            });


            result.Succeeded.ShouldBe(true);
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Data.ResetPasswordToken.ShouldNotBeNullOrEmpty();
            result.Data.UserId.ShouldNotBeNullOrEmpty();
        }


        [Fact]
        public async Task Handle_WhenUserNotFound_ReturnNotFoundAndErrorMessage()
        {
            var command = new ForgotPasswordCommand(email: "Error@error.com");
            var validationBehavior = new ValidationBehaviour<ForgotPasswordCommand, ApiResponse<ForgotPasswordResponse>>(new List<ForgotPasswordCommandValidator>()
            {
                _validator
            });
            var result = await validationBehavior.Handle(command, CancellationToken.None, () =>
            {
                return _handler.Handle(command, cancellationToken: CancellationToken.None);
            });


            result.Succeeded.ShouldBe(false);
            result.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
            result.ErrorMessages.ShouldNotBeEmpty();
            result.Data.ShouldBeNull();
            result.DataList.ShouldBeEmpty();
        }

        [Fact]
        public async Task Handle_WhenUserNotConfirmed_ReturnsUnhautorized_AndErrorMessage()
        {
            var command = new ForgotPasswordCommand(email: "Error2@error.com");
            var validationBehavior = new ValidationBehaviour<ForgotPasswordCommand, ApiResponse<ForgotPasswordResponse>>(new List<ForgotPasswordCommandValidator>()
            {
                _validator
            });
            var result = await validationBehavior.Handle(command, CancellationToken.None, () =>
            {
                return _handler.Handle(command, cancellationToken: CancellationToken.None);
            });


            result.Succeeded.ShouldBe(false);
            result.StatusCode.ShouldBe((int)HttpStatusCode.Unauthorized);
            result.ErrorMessages.ShouldNotBeEmpty();
            result.Data.ShouldBeNull();
            result.DataList.ShouldBeNull();
        }


        [Theory]
        [ClassData(typeof(ForgotPassword_BadRequest_Command))]
        public async Task Handle_WithInvalidInput_ReturnsBadRequest_AndErrorMessage(string email)
        {
            var command = new ForgotPasswordCommand(email: email);
            var validationBehavior = new ValidationBehaviour<ForgotPasswordCommand, ApiResponse<ForgotPasswordResponse>>(new List<ForgotPasswordCommandValidator>()
            {
                _validator
            });
            var result = await validationBehavior.Handle(command, CancellationToken.None, () =>
            {
                return _handler.Handle(command, cancellationToken: CancellationToken.None);
            });


            result.Succeeded.ShouldBe(false);
            result.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            result.ErrorMessages.ShouldNotBeEmpty();
            result.Data.ShouldBeNull();
            result.DataList.ShouldBeNull();
        }

    }
}
