using System;
using System.Threading.Tasks;
using Template.Domain.Entities;

namespace Template.Application.Contrats.Persistence
{
    public interface ICategoryRepository : IAsyncRepository<Category>
    {
        Task<Category> getWithEvents(bool includeHistory, Guid id);
        Task<bool> IsNameUnique(string categoryName);
        Task<bool> IsNameUniqueForUpdate(Guid id, string categoryName);
    }
}
