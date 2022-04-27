using System.Collections;
using System.Collections.Generic;
using Template.Application.Features.Categories;
using Template.Application.Features.Categories.Commands.Create;
using Template.Application.Features.Categories.Commands.Update;
using Template.Application.Features.Categories.Queries.GetCategoryWithEvent;
using Template.Application.Features.Events.Commands.CreateEvent;
using Template.Application.Features.Events.Commands.UpdateEvent;
using Template.Application.Features.Events.Queries.GetEventDetails;
using Template.Application.Features.Events.Queries.GetEventExport;
using Template.Application.Features.Events.Queries.GetEventList;
using Template.Domain.Entities;

namespace Application.UnitTests.MappingProfiles
{
    public static class MappingClassData
    {
        public class MappingSets : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
                {
                    new object[] {typeof(Event),typeof(EventListVm) },
                    new object[] {typeof(Event),typeof(EventDetailVm) },
                    new object[] {typeof(Event),typeof(EventExportDto) },
                    new object[] {typeof(Event),typeof(EventDto) },
                    new object[] {typeof(CreateEventCommand),typeof(Event) },
                    new object[] {typeof(Event),typeof(EventListVm) },
                    new object[] {typeof(UpdateEventCommand),typeof(Event) },

                    new object[] {typeof(Category),typeof(CategoryVm) },
                    new object[] {typeof(Category),typeof(CategoryWithEventsVm) },
                    new object[] {typeof(CreateCategoryCommand),typeof(Category) },
                    new object[] {typeof(UpdateCategoryCommand),typeof(Category) },
                };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
