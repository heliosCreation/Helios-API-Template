using System;
using Template.Application.Features.Categories;

namespace Template.Application.Features.Events.Queries.GetEventDetails
{
    public class EventDetailVm
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Artist { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public CategoryVm Category { get; set; }
    }
}