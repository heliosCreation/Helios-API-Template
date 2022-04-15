using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contracts.Infrastructure;
using Template.Application.Contrats.Persistence;
using Template.Application.Responses;
using Template.Domain.Entities;

namespace Template.Application.Features.Events.Queries.GetEventExport
{

    public class GetEventExportQueryHandler : IRequestHandler<GetEventExportQuery, ApiResponse<EventExportFileVm>>
    {
        private readonly IAsyncRepository<Event> _eventRepository;
        private readonly ICsvExporterService _csvExporter;
        private readonly IMapper _mapper;

        public GetEventExportQueryHandler(
            IAsyncRepository<Event> eventRepository,
            ICsvExporterService csvExporter,
            IMapper mapper
            )
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _csvExporter = csvExporter ?? throw new ArgumentNullException(nameof(csvExporter));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse<EventExportFileVm>> Handle(GetEventExportQuery request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<EventExportFileVm>();
            var eventDto = _mapper.Map<List<EventExportDto>>((await _eventRepository.ListAllAsync()).OrderBy(e => e.Date));
            var csv = _csvExporter.ExportEventToCsv(eventDto);
            response.Data = new EventExportFileVm
            {
                EventExportFileName = $"Current list of event as for {DateTime.Today}",
                ContentType = "text/csv",
                Data = csv
            };
            return response;
        }
    }
}
