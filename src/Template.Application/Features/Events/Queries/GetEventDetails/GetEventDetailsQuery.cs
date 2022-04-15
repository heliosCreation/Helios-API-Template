using MediatR;
using System;
using Template.Application.Responses;

namespace Template.Application.Features.Events.Queries.GetEventDetails
{
    public class GetEventDetailsQuery : IRequest<ApiResponse<EventDetailVm>>
    {
        public GetEventDetailsQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }

    }
}
