using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;
using Template.Application.Features.Events.Queries.GetEventList;
using Template.Application.Responses;
using Template.Domain.Entities;

namespace GloboEvent.Application.Features.Events.Queries.GetEventList
{
    public class GetEventListQueryHandler : IRequestHandler<GetEventListQuery, ApiResponse<EventListVm>>
    {
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;

        public GetEventListQueryHandler(
            IMapper mapper,
             IEventRepository eventRepository)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
        }
        public async Task<ApiResponse<EventListVm>> Handle(GetEventListQuery request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<EventListVm>();
            var allEvent = new List<Event>();
            if (request.IncludeHistory)
            {
                allEvent = await _eventRepository.GetTodayEvents();
            }
            else
            {
                allEvent = (await _eventRepository.ListAllAsync()).OrderBy(x => x.Date).ToList();
            }
            response.DataList = _mapper.Map<List<EventListVm>>(allEvent);
            return response;
        }
    }
}
