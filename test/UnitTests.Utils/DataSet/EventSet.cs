using System;
using System.Collections;
using System.Collections.Generic;
using Template.Domain.Entities;

namespace UnitTests.Utils.DataSet
{
    using static CategorySet;
    public static class EventSet
    {
        public static Guid EventId1 { get; set; } = Guid.Parse("dd979d92-fabb-4c5e-ad02-9f1e55e358e6");
        public static string EventName1 { get; set; } = ".Net 5 conference";
        public static Guid EventId2 { get; set; } = Guid.Parse("8dfcb9ee-e37a-4c99-9d5c-5ba75ee9b6ad");
        public static string EventName2 { get; set; } = "Azure in 2021";

        public static Event NewEvent { get; set; } = new Event
        {
            Name = "Clean Architecture",
            Date = DateTime.Today.AddDays(3),
            Price = 25,
            CategoryId = CategoryId1
        };


        public class CreateEventInvalidCommand : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
                {
                    new object[] { "", 900, DateTime.Now.AddDays(1),CategoryId1},//Bad name
                    new object[] {  new string('*', 51), 900, DateTime.Now.AddDays(1),CategoryId1},
                    new object[] { EventName1, 900, DateTime.UtcNow.Date,CategoryId1}, //Same event same day
                    new object[] { "", -1, DateTime.Now.AddDays(1),CategoryId1},//Bad Price
                    new object[] { "", 900, DateTime.Now.AddDays(-1),CategoryId1},//Wrong Date
                    new object[] { "", 900, DateTime.Now.AddDays(1),Guid.NewGuid()}, // Wrong CategoryId

                };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


        public class UpdateEventInvalidCommand : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
                {
                    new object[] {null, "", 900, DateTime.Now.AddDays(1),CategoryId1},//Bad name
                    new object[] {EventId1, "", 900, DateTime.Now.AddDays(1),CategoryId1},//Bad name
                    new object[] {EventId1, new string('*', 51), 900, DateTime.Now.AddDays(1), CategoryId1},
                    new object[] {EventId1,EventName1, 900, DateTime.UtcNow.Date,CategoryId1}, //Same event same day
                    new object[] {EventId1,"Test", -1, DateTime.Now.AddDays(1),CategoryId1},//Bad Price
                    new object[] {EventId1, "Test", 900, DateTime.Now.AddDays(-1),CategoryId1},//Wrong Date
                    new object[] {EventId1, "Test", 900, DateTime.Now.AddDays(1),Guid.NewGuid() }, // Wrong CategoryId

                };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

    }
}
