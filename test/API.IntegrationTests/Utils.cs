using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace Api.IntegrationTests
{
    public static class Utils
    {
        public static class CategoryTools
        {
            public class UpdateCategoryInvalidCommand : IEnumerable<object[]>
            {
                private readonly List<object[]> _data = new List<object[]>
                {
                    new object[] { "B0788D2F-8003-43C1-92A4-EDC76A7C5DDE", new string('*', 5000), (int)HttpStatusCode.BadRequest},
                    new object[] { "B0788D2F-8003-43C1-92A4-EDC76A7C5DDE","Musicals",(int) HttpStatusCode.BadRequest },
                    new object[] { "B0788D2F-8003-43C1-92A4-EDC76A7C5DDE","", (int)HttpStatusCode.BadRequest},
                    new object[] { Guid.NewGuid(),"Test",(int)HttpStatusCode.NotFound }
                };

                public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }

            public class CreateCategoryInvalidCommand : IEnumerable<object[]>
            {
                private readonly List<object[]> _data = new List<object[]>
                {
                    new object[] {new string('*', 5000)},
                    new object[] { "Musicals" },
                    new object[] { "" },
                };

                public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }

            public class DeleteCategoryCommandData : IEnumerable<object[]>
            {
                private readonly List<object[]> _data = new List<object[]>
                {
                    new object[] { "62787623-4C52-43FE-B0C9-B7044FB5929B", (int)HttpStatusCode.NotFound},
                    new object[] { "B0788D2F-8003-43C1-92A4-EDC76A7C5DDE",(int) HttpStatusCode.OK},
                };

                public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }
        }

        public static class EventTools
        {
            public class CreateEventInvalidCommand : IEnumerable<object[]>
            {
                private readonly List<object[]> _data = new List<object[]>
                {
                    new object[] { "", 900, DateTime.Now.AddDays(1),"B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"},//Bad name
                    new object[] {  new string('*', 51), 900, DateTime.Now.AddDays(1),"B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"},
                    new object[] { "John Egbert Live", 900, DateTime.UtcNow.Date,"B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"}, //Same event same day
                    new object[] { "", 0, DateTime.Now.AddDays(1),"B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"},//Bad Price
                    new object[] { "", -1, DateTime.Now.AddDays(1),"B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"},
                    new object[] { "", 900, DateTime.Now.AddDays(-1),"B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"},//Wrong Date
                    new object[] { "", 900, DateTime.Now.AddDays(1),"EE272F8B-6096-4CB6-8625-BB4BB2D89E8B"}, // Wrong CategoryId

                };

                public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }
            public class UpdateEventInvalidCommand : IEnumerable<object[]>
            {
                private readonly List<object[]> _data = new List<object[]>
                {
                    new object[] { "", 900, DateTime.Now.AddDays(1),"B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"},//Bad name
                    new object[] {  new string('*', 51), 900, DateTime.Now.AddDays(1),"B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"},
                    new object[] { "Clash of the DJs", 900, DateTime.UtcNow.Date,"B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"}, //Same event same day
                    new object[] { "", 0, DateTime.Now.AddDays(1),"B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"},//Bad Price
                    new object[] { "", -1, DateTime.Now.AddDays(1),"B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"},
                    new object[] { "", 900, DateTime.Now.AddDays(-1),"B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"},//Wrong Date
                    new object[] { "", 900, DateTime.Now.AddDays(1),"EE272F8B-6096-4CB6-8625-BB4BB2D89E8B"}, // Wrong CategoryId

                };

                public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }

            public class DeleteEventCommands : IEnumerable<object[]>
            {
                private readonly List<object[]> _data = new List<object[]>
                {
                    new object[] { "62787623-4C52-43FE-B0C9-B7044FB5929B", (int)HttpStatusCode.OK},
                    new object[] { "B0788D2F-8003-43C1-92A4-EDC76A7C5DDE",(int) HttpStatusCode.NotFound},
                };

                public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }
        }

        public static class AccountTools
        {
            public class RegisterValidationTest : IEnumerable<object[]>
            {
                private readonly List<object[]> _data = new List<object[]>
                {
                    new object[] { "","Doe","JohnDoe","JohnDoe@gmail.com","Pwd12345!"},//Check Required fields
                    new object[] { "John","","JohnDoe","JohnDoe@gmail.com","Pwd12345!"},
                    new object[] { "John", "Doe","","JohnDoe@gmail.com","Pwd12345!"},
                    new object[] { "John", "Doe","JohnDoe","","Pwd12345!"},
                    new object[] { "John", "Doe","JohnDoe","JohnDoe@gmail.com",""},
                    new object[] { new string('*', 121), "Doe","JohnDoe","JohnDoe@gmail.com","Pwd12345!"},//Name length
                    new object[] { "John", new string('*',121),"JohnDoe","JohnDoe@gmail.com","Pwd12345!"},//LastName length
                    new object[] { "John", new string('*',121),"JohnDoe","JohnDoe@gmail.com","Pwd12345!"},//Username length
                    new object[] { "John", "Doe", "JohnDoe", string.Concat(new string('*',111),"@gmail.com"), "Pwd12345!" },//Email length
                    new object[] { "John", "Doe", "JohnDoe", "JohnDoegmail.com","Pwd12345!"},//Email Type
                    new object[] { "John", "Doe", "JohnDoe", "JohnDoe@gmail.com",string.Concat(new string('*',120),"Pwd12345!")},//PasswordLength
                    new object[] { "John","Doe","JohnDoe","JohnDoe@gmail.com","pwd12345!"},//Password Uppercase
                    new object[] { "John","Doe","JohnDoe","JohnDoe@gmail.com","Pwdpwdpwd!"},//Password digit
                    new object[] { "John","Doe","JohnDoe","JohnDoe@gmail.com","Pwd12345"},//Password Non alphabetical value

                };

                public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }
        }
    }
}
