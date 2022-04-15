using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Domain.Entities;

namespace Template.Application.Contrats.Persistence
{
    public interface IEventRepository : IAsyncRepository<Event>
    {
        Task<List<Event>> GetTodayEvents();
        Task<bool> IsUniqueNameAndDate(string name, DateTime date);
        Task<bool> IsUniqueNameAndDateForUpdate(string name, DateTime date, Guid id);
    }
}
