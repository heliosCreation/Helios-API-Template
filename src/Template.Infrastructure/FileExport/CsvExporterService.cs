using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Template.Application.Contracts.Infrastructure;
using Template.Application.Features.Events.Queries.GetEventExport;

namespace Template.Infrastructure.FileExport
{
    public class CsvExporterService : ICsvExporterService
    {
        public byte[] ExportEventToCsv(List<EventExportDto> eventExportDtos)
        {
            using var memoryStream = new MemoryStream();
            using var streamWritter = new StreamWriter(memoryStream);
            using var csvWriter = new CsvWriter(streamWritter, CultureInfo.GetCultureInfo("fr-FR"));

            //csvWriter.WriteField("EventId");
            //csvWriter.WriteField("Name");
            //csvWriter.WriteField("Date");
            //csvWriter.NextRecord();

            //foreach (var @event in eventExportDtos)
            //{
            //    csvWriter.WriteField(@event.EventId);
            //    csvWriter.WriteField(@event.Name);
            //    csvWriter.WriteField(@event.Date);
            //    csvWriter.NextRecord();
            //}
            csvWriter.WriteRecords(eventExportDtos);


            streamWritter.Flush();
            var result = memoryStream.ToArray();

            return result;

        }
    }
}
