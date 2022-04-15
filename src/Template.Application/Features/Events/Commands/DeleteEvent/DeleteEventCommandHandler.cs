using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;
using Template.Application.Responses;
using Template.Domain.Entities;

namespace Template.Application.Features.Events.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, ApiResponse<object>>
    {
        private readonly IAsyncRepository<Event> _eventRepoistory;

        public DeleteEventCommandHandler(
            IAsyncRepository<Event> eventRepoistory)
        {
            _eventRepoistory = eventRepoistory ?? throw new ArgumentNullException(nameof(eventRepoistory));
        }
        public async Task<ApiResponse<object>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<object>();
            var eventToDelete = await _eventRepoistory.GetByIdAsync(request.EventId);
            if (eventToDelete == null)
            {
                return response.setNotFoundResponse();
            }
            await _eventRepoistory.DeleteAsync(eventToDelete);
            return response;
        }
    }
}
