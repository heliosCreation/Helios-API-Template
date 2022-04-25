//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Template.Application.Contracts.Identity;
//using Template.Application.Model.Account;

//namespace Application.UnitTests.Mocks.Identity
//{
//    public class MockTokenUtilsService
//    {
//        public Mock<ITokenUtils> MockService { get; set; } = new Mock<ITokenUtils>();

//        public Mock<ITokenUtils> GetService()
//        {
//            MockService.Setup(s => s.GenerateAuthenticationResponseForUserAsync(It.IsAny<string>(), It.IsAny<JwtSettings>())).Returns(() =>
//            {

//            });

//            return MockService;
//        }

//    }
//}
