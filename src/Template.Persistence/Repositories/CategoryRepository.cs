using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;
using Template.Domain.Entities;

namespace Template.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {

        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Category> getWithEvents(bool includeHistory, Guid id)
        {
            if (includeHistory)
            {
                return await _dbContext
                    .Categories
                    .Where(c => c.Id == id)
                    .Include(c => c.Events)
                    .FirstOrDefaultAsync();
            }
            else
            {
                return await _dbContext.Categories
                 .Where(c => c.Id == id)
                .Include(c => c.Events.Where(c => c.Date.Date == DateTime.Today.Date))
                .FirstOrDefaultAsync();
            }
        }

        public async Task<bool> IsNameUnique(string categoryName)
        {
            var isUnique = await _dbContext.Categories.AnyAsync(c => c.Name == categoryName) == false;
            return isUnique;
        }
        public async Task<bool> IsNameUniqueForUpdate(Guid id, string categoryName)
        {
            return !await _dbContext.Categories.AnyAsync(c => c.Name == categoryName && c.Id != id);
        }
    }
}
