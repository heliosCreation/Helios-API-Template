using System;

namespace Template.Application.Features.Events.Queries.GetEventExport
{
    public class EventExportDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
