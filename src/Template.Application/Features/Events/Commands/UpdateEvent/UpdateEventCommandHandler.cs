using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;
using Template.Application.Responses;
using Template.Domain.Entities;

namespace Template.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, ApiResponse<object>>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Event> _eventRepository;

        public UpdateEventCommandHandler
            (
            IMapper mapper,
            IAsyncRepository<Event> eventRepository
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }
        public async Task<ApiResponse<object>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<object>();
            var eventToUpdate = await _eventRepository.GetByIdAsync(request.Id);
            if (eventToUpdate == null)
            {
                return response.setNotFoundResponse();
            }
            _mapper.Map(request, eventToUpdate, typeof(UpdateEventCommand), typeof(Event));
            var success = await _eventRepository.UpdateAsync(eventToUpdate);
            if (! success)
            {
                return response.SetInternalServerErrorResponse("There was an error trying to save your data. If the problem persists, contact an administrator");
            }
            return response;

        }
    }
}
