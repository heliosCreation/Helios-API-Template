using Api.IntegrationTest.Base;
using Api.IntegrationTests;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Template.API.Contract;
using Template.Application.Features.Events.Commands.CreateEvent;
using Template.Application.Features.Events.Commands.UpdateEvent;
using Template.Application.Features.Events.Queries.GetEventDetails;
using Template.Application.Responses;
using Xunit;

namespace GloboEvent.Api.IntegrationTest.Controllers.Events.Commands
{
    using static ApiRoutes.Event;
    using static Utils.EventTools;

    public class EventController_CommandsTest : IntegrationTestBase
    {
        public string DefaultCategoryId { get; set; } = "B0788D2F-8003-43C1-92A4-EDC76A7C5DDE";
        public string CategoryForUpdateId { get; set; } = "FE98F549-E790-4E9F-AA16-18C2292A2EE9";
        public string DefaultEventId { get; set; } = "EE272F8B-6096-4CB6-8625-BB4BB2D89E8B";

        #region Create
        [Fact]
        public async Task AddCategory_WhenValid_ShouldReturnCorrectStatusCode()
        {
            await AuthenticateAsync();

            var response = await TestClient.PostAsJsonAsync(
                Create,
                new CreateEventCommand { Name = "TestEvent", Price = 900, Date = DateTime.Now.AddDays(2), CategoryId = Guid.Parse(DefaultCategoryId) });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [ClassData(typeof(CreateEventInvalidCommand))]
        public async Task AddCategory_ShouldReturnsBadRequest_WhenInvalidFieldProvided(string name, int price, DateTime date, string categoryId)
        {
            await AuthenticateAsync();

            var response = await TestClient.PostAsJsonAsync(
                Create,
                new CreateEventCommand { Name = name, Price = price, Date = date, CategoryId = Guid.Parse(categoryId) });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Update
        [Fact]
        public async Task UpdateEvent_shouldReturnsNoContent_whenValid()
        {
            await AuthenticateAsync();

            var response = await TestClient.PutAsJsonAsync(
                Update.Replace("{id}", DefaultEventId),
                new UpdateEventCommand { Id = Guid.Parse(DefaultEventId), Name = "Updated", Price = 1, Date = DateTime.UtcNow.Date.AddDays(1), CategoryId = Guid.Parse(CategoryForUpdateId) });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateEvent_shouldUpdateEventFields()
        {
            await AuthenticateAsync();

            await TestClient.PutAsJsonAsync(
            Update.Replace("{id}", DefaultEventId),
            new UpdateEventCommand { Id = Guid.Parse(DefaultEventId), Name = "Updated", Price = 1, Date = DateTime.UtcNow.Date.AddDays(1), CategoryId = Guid.Parse(CategoryForUpdateId) });

            var response = await TestClient.GetAsync(GetById.Replace("{id}", DefaultEventId));
            var content = await response.Content.ReadAsAsync<ApiResponse<EventDetailVm>>();

            content.Data.Name.Should().Be("Updated");
            content.Data.Price.Should().Be(1);
            content.Data.Date.Should().Be(DateTime.UtcNow.Date.AddDays(1));
            content.Data.Category.Id.Should().Be(Guid.Parse(CategoryForUpdateId));
        }

        [Theory]
        [ClassData(typeof(UpdateEventInvalidCommand))]
        public async Task UpdateEvent_ShouldReturnBadRequest_WhenInvalidFieldsProvided(string name, int price, DateTime date, string categoryId)
        {
            await AuthenticateAsync();

            var response = await TestClient.PutAsJsonAsync(
                Update.Replace("{id}", DefaultEventId),
                new UpdateEventCommand { Id = Guid.Parse(DefaultEventId), Name = name, Price = price, Date = date, CategoryId = Guid.Parse(categoryId) });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateEvent_ShouldReturnNotFound_WhenInvalidIdProvided()
        {
            await AuthenticateAsync();

            var response = await TestClient.PutAsJsonAsync(
                Update.Replace("{id}", DefaultEventId),
                new UpdateEventCommand { Id = Guid.Parse(DefaultCategoryId), Name = "Test", Price = 900, Date = DateTime.UtcNow.AddDays(1), CategoryId = Guid.Parse(DefaultCategoryId) });

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        #endregion

        #region Delete
        [Theory]
        [ClassData(typeof(DeleteEventCommands))]
        public async Task DeleteEvent_Should_returnsValidStatusCode_DependingOnFoundOrNot(string id, int result)
        {
            await AuthenticateAsync();

            var response = await TestClient.DeleteAsync(Delete.Replace("{id}", id));

            ((int)response.StatusCode).Should().Be(result);

        }

        [Fact]
        public async Task GetEventById_Should_returnsNotFound_afterDeletingEvent()
        {
            await AuthenticateAsync();

            await TestClient.DeleteAsync(Delete.Replace("{id}", DefaultEventId));
            var response = await TestClient.GetAsync(GetById.Replace("{id}", DefaultEventId));
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }
        #endregion
    }
}
