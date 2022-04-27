using Application.UnitTests.Mocks.Identity;
using Application.UnitTests.Mocks.Infrastructure;
using Moq;
using Template.Application.Contracts.Identity;
using Template.Application.Contracts.Infrastructure;

namespace Application.UnitTests.Account
{
    public class AccountBaseTest
    {
        protected readonly Mock<IAuthenticationService> _mockAuthenticationService;
        protected readonly Mock<IEmailService> _mockEmailService;

        public AccountBaseTest()
        {
            _mockAuthenticationService = new MockAuthenticationService().GetEntityRepository();
            _mockEmailService = new MockEmailService().GetEntityRepository();
        }
    }
}
