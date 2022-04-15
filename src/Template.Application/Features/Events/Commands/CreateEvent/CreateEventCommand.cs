using MediatR;
using System;
using Template.Application.Responses;

namespace Template.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommand : IRequest<ApiResponse<CreateEventResponse>>
    {
        public string Name { get; set; }

        public int Price { get; set; }

        public string Artist { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public Guid CategoryId { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Price: {Price}, Artist: {Artist}, On: {Date.ToShortDateString()}, Description:{Description}";
        }
    }
}
