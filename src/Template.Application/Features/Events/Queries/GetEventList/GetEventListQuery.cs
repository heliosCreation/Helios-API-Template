using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Events.Queries.GetEventList
{
    public class GetEventListQuery : IRequest<ApiResponse<EventListVm>>
    {
        public bool IncludeHistory { get; set; }

        public GetEventListQuery(bool includeHistory)
        {
            IncludeHistory = includeHistory;
        }
    }
}
