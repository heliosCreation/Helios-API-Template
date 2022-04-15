using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;
using Template.Application.Responses;

namespace Template.Application.Features.Events.Queries.GetEventDetails
{
    public class GetEventtDetailQueryHandler : IRequestHandler<GetEventDetailsQuery, ApiResponse<EventDetailVm>>
    {
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;

        public GetEventtDetailQueryHandler(
            IMapper mapper,
            IEventRepository eventRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }

        public async Task<ApiResponse<EventDetailVm>> Handle(GetEventDetailsQuery request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<EventDetailVm>();
            var @event = await _eventRepository.GetByIdAsync(request.Id);
            if (@event == null)
            {
                return response.setNotFoundResponse();
            }
            var eventDetailDto = _mapper.Map<EventDetailVm>(@event);

            response.Data = eventDetailDto;

            return response; ;
        }
    }
}
