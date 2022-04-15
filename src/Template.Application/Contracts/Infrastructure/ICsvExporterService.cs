using System.Collections.Generic;
using Template.Application.Features.Events.Queries.GetEventExport;

namespace Template.Application.Contracts.Infrastructure
{
    public interface ICsvExporterService
    {
        byte[] ExportEventToCsv(List<EventExportDto> eventExportDtos);
    }
}
