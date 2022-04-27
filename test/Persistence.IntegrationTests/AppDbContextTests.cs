using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Threading.Tasks;
using Template.Application.Contrats;
using Template.Domain.Entities;
using Template.Persistence;
using Xunit;

namespace Persistence.IntegrationTests
{
    public class AppDbContextTests
    {
        private readonly AppDbContext _dbContext;
        private readonly Mock<ILoggedInUserService> _loggedInUserServiceMock;
        private readonly string _userId;

        public AppDbContextTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            _userId = "4223E4CA-8C28-4623-9470-ED316D1D2E58";
            _loggedInUserServiceMock = new Mock<ILoggedInUserService>();
            _loggedInUserServiceMock.Setup(m => m.UserId).Returns(_userId);

            _dbContext = new AppDbContext(dbContextOptions.Options, _loggedInUserServiceMock.Object);
        }

        [Fact]
        public async Task Save_SetCreatedBy_AndCreatedDate_Properties()
        {
            var @event = new Event { Id = Guid.NewGuid(), Name = "Test Event" };

            _dbContext.Events.Add(@event);
            await _dbContext.SaveChangesAsync();

            @event.CreatedBy.ShouldBe(_userId);
            (DateTime.UtcNow - @event.CreatedDate).ShouldBeLessThan(TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task Update_LastModifiedBy_AndUpdatedDate_Properties()
        {
            var id = Guid.NewGuid();
            var @event = new Event { Id = id , Name = "Test Event" };
            _dbContext.Events.Add(@event);
            await _dbContext.SaveChangesAsync();

            _dbContext.Entry(@event).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            @event.LastModifiedBy.ShouldBe(_userId);
            (DateTime.UtcNow - (DateTime)@event.LastModifiedDate).ShouldBeLessThan(TimeSpan.FromSeconds(1));
        }
    }
}
