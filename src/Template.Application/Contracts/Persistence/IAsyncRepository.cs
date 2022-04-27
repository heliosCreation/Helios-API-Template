using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template.Application.Contrats.Persistence
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T> AddAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task DeleteAsync(T entity);

    }
}
