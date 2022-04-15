using System;

namespace Template.Application.Features.Categories.Queries.GetCategoryWithEvent
{
    public class EventDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Artist { get; set; }

        public DateTime Date { get; set; }

        public Guid CategoryId { get; set; }
    }
}