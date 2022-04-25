using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace UnitTests.Utils.DataSet
{
    public static class CategorySet
    {
        public static Guid CategoryId1 { get; set; } = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
        public static string CategoryName1 { get; set; } = "Plays";
        public static Guid CategoryId2 { get; set; } = Guid.Parse("{62787623-4C52-43FE-B0C9-B7044FB5929B}");
        public static string CategoryName2 { get; set; } = "Musicals";

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

        public class UpdateCategoryInvalidCommand : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] { CategoryId1, new string('*', 101), (int)HttpStatusCode.BadRequest},
                new object[] { CategoryId1,"Musicals",(int)HttpStatusCode.BadRequest},
                new object[] { CategoryId1, "",(int)HttpStatusCode.BadRequest },
                new object[] { Guid.NewGuid().ToString(),"Test", (int)HttpStatusCode.NotFound }
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class DeleteCategoryCommandData : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] { Guid.NewGuid(), (int)HttpStatusCode.NotFound},
                new object[] { CategoryId1,(int) HttpStatusCode.OK},
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}

