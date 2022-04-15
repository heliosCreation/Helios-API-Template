using MediatR;
using System;
using Template.Application.Responses;

namespace Template.Application.Features.Events.Commands.DeleteEvent
{
    public class DeleteEventCommand : IRequest<ApiResponse<object>>
    {
        public DeleteEventCommand(Guid eventId)
        {
            EventId = eventId;
        }
        public Guid EventId { get; set; }
    }
}
