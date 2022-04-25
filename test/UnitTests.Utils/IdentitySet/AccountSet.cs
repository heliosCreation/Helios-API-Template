using System.Collections;
using System.Collections.Generic;

namespace UnitTests.Utils.IdentitySet
{
    public static class AccountSet
    {
        #region authenticate
        public class Authenticate_BadRequest_Command : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] {new string('*', 121), "testpwd"},
                new object[] { "", "testPwd" },
                new object[] { "testemail", "testPwd" },
                new object[] { "test@test.com","" },
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion

        #region register
        public class Register_BadRequest_Command : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] { "", "Doe","a@b.com","jdoe","testpwd!","testpwd!" },
                new object[] { "John", "","a@b.com","jdoe","testpwd!","testpwd!" },
                new object[] { "John", "Doe","ab.com","jdoe","testpwd!","testpwd!" },
                new object[] { "John", "Doe","ab@.com","","testpwd!","testpwd!" },
                new object[] { "John", "Doe","ab@.com","jdoe","testpw!","testpwd!" },
                new object[] { "John", "Doe","ab@.com","Exist","testpwd!","testpwd!" },
                new object[] { "John", "Doe","Exist@test.com","Exist","testpwd!","testpwd!" },
                new object[] { "Error", "Doe","Exist@test.com","Exist","testpwd!","testpwd!" },
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion

        #region confirmEmail
        public class ConfirmEmail_BadRequest_Command : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] {"", "token"},
                new object[] {"test", "token"},
                new object[] {"test@test.com", ""},

            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        #endregion
    }
}
