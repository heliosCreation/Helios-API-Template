using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;
using Template.Domain.Entities;

namespace Template.Persistence.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
        public override async Task<Event> GetByIdAsync(Guid id)
        {
            var x = await _dbContext.Events
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

            return x;
        }

        public async Task<List<Event>> GetTodayEvents()
        {
            return await _dbContext.Events
                .Include(e => e.Category)
                .Where(e => e.Date.Date == DateTime.Today.Date)
                .ToListAsync();
        }

        public async Task<bool> IsUniqueNameAndDate(string name, DateTime date)
        {
            var matches = await _dbContext.Events.AnyAsync(e => e.Name.Equals(name) && e.Date.Equals(date));
            return matches == false;
        }

        public async Task<bool> IsUniqueNameAndDateForUpdate(string name, DateTime date, Guid id)
        {
            var matches = await _dbContext.Events.AnyAsync(e => e.Name.Equals(name) && e.Date.Equals(date) && e.Id != id);
            return matches == false;
        }
    }
}
