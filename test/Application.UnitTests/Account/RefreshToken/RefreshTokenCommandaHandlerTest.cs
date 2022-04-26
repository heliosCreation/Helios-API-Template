using Template.Application.Features.Account.Command.RefreshToken;

namespace Application.UnitTests.Account.RefreshToken
{
    public class RefreshTokenCommandaHandlerTest : AccountBaseTest
    {
        private readonly ResfreshTokenCommandHandler _handler;
        private readonly RefreshTokenCommandValidator _validator;

        public RefreshTokenCommandaHandlerTest()
        {
            _handler = new ResfreshTokenCommandHandler(_mockAuthenticationService.Object);
            _validator = new RefreshTokenCommandValidator();

        }

    }
}
