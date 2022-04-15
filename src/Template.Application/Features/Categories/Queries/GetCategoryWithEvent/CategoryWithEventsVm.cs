using System;
using System.Collections.Generic;

namespace Template.Application.Features.Categories.Queries.GetCategoryWithEvent
{
    public class CategoryWithEventsVm
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<EventDto> Events { get; set; }
    }
}
