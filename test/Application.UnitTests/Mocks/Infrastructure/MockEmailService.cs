using Moq;
using Template.Application.Contracts.Infrastructure;

namespace Application.UnitTests.Mocks.Infrastructure
{
    public class MockEmailService
    {
        public Mock<IEmailService> MockService { get; set; } = new Mock<IEmailService>();

        public Mock<IEmailService> GetEntityRepository()
        {
            MockService.Setup(s => s.SendForgotPasswordMail(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            MockService.Setup(s => s.SendForgotPasswordMail(It.Is<string>(s => s == "test@Error.com"), It.IsAny<string>())).ReturnsAsync(false);

            return MockService;
        }

    }
}
