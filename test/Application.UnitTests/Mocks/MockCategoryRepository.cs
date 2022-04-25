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
    using static CategorySet;
    public class MockCategoryRepository : MockBaseExtension<Category, ICategoryRepository>
    {
        public override Mock<ICategoryRepository> GetEntityRepository()
        {
            setCategoriesData();

            MockRepo.Setup(r => r.getWithEvents(It.IsAny<bool>(), It.IsAny<Guid>())).ReturnsAsync((bool includeHistory, Guid id) =>
            {
                if (includeHistory)
                {
                    return Entities
                    .Where(c => c.Id == id)
                    .FirstOrDefault();
                }
                else
                {
                    var result = Entities
                    .Where(c => c.Id == id)
                    .FirstOrDefault();
                    if (result != null)
                    {
                        var holder = new List<Event>();

                        foreach (var @event in result.Events)
                        {
                            if (@event.Date.Date == DateTime.Today.Date)
                            {
                                holder.Add(@event);
                            }
                        }
                        result.Events = holder;
                    }
                    return result;
                }
            });

            MockRepo.Setup(r => r.IsNameUnique(It.IsAny<string>())).ReturnsAsync((string name) =>
            {
                var check = !Entities.Any(e => e.Name == name);
                return check;
            });

            MockRepo.Setup(r => r.IsNameUniqueForUpdate(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync((Guid id, string name) =>
            {
                var check = !Entities.Any(e => e.Name == name && e.Id != id);
                return check;
            });
            return base.GetEntityRepository();
        }

        private void setCategoriesData()
        {
            Entities = new List<Category>()
            {

                new Category
                {
                    Id = CategoryId1, Name = CategoryName1,
                    Events = new List<Event>()
                    {
                        new Event { Date = DateTime.Today, CategoryId = CategoryId1 },
                        new Event { Date = DateTime.Today.AddDays(1), CategoryId = CategoryId1 }
                    }
                },

                new Category
                {
                    Id = CategoryId2, Name = CategoryName2,
                    Events = new List<Event>()
                    {
                        new Event { Date = DateTime.Today, CategoryId = CategoryId2 },
                        new Event { Date = DateTime.Today.AddDays(1), CategoryId = CategoryId2 }
                    }
                }
            };
        }
    }
}



