using Application.UnitTests.Mocks.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Contracts.Identity;
using static UnitTests.Utils.IdentitySet.AccountSet;

namespace Application.UnitTests.Account
{
    public class AccountBaseTest
    {
        protected readonly Mock<IAuthenticationService> _mockAuthenticationService;

        public AccountBaseTest()
        {
            _mockAuthenticationService = new MockAuthenticationService().GetEntityRepository();
        }
    }
}
