using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Events.Queries.GetEventExport
{
    public class GetEventExportQuery : IRequest<ApiResponse<EventExportFileVm>>
    {
    }
}
