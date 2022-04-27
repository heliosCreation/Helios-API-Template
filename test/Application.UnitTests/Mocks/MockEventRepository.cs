using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Template.Application.Contrats.Persistence;
using Template.Domain.Entities;
using Tests.Utils.Mock;
using UnitTests.Utils.DataSet;

namespace Application.UnitTests.Mocks
{
    using static EventSet;

    public class MockEventRepository : MockBaseExtension<Event, IEventRepository>
    {
        public override Mock<IEventRepository> GetEntityRepository()
        {
            setEventsData();

            MockRepo.Setup(r => r.GetTodayEvents())
                .ReturnsAsync(() =>
                {
                    return Entities.Where(e => e.Date.Date == DateTime.Today.Date).ToList();
                });

            MockRepo.Setup(r => r.IsUniqueNameAndDate(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync((string name, DateTime date) =>
                {
                    return !Entities.Any(e => e.Name == name && e.Date.Date == date.Date);
                });

            MockRepo.Setup(r => r.IsUniqueNameAndDateForUpdate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<Guid>()))
                .ReturnsAsync((string name, DateTime date, Guid id) =>
                {
                    return !Entities.Any(e => e.Name == name && e.Date.Date == date.Date && e.Id != id);
                });

            return base.GetEntityRepository();
        }

        private void setEventsData()
        {
            Entities = new List<Event>()
            {
                new Event
                {
                    Id = EventId1,
                    Date = DateTime.Today,
                    Artist = "Jason Taylor",
                    Name = EventName1,
                    CategoryId = CategorySet.CategoryId1,
                    Price = 10
                },
                new Event
                {
                    Id = EventId2,
                    Date = DateTime.Today.AddDays(1),
                    Artist = "Jason Taylor",
                    Name = EventName2,
                    CategoryId = CategorySet.CategoryId1,
                    Price = 25
                }
            };
        }

    }
}
