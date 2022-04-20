using Api.IntegrationTest.Base;
using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Template.API.Contract;
using Template.Application.Features.Events.Commands.CreateEvent;
using Template.Application.Features.Events.Queries.GetEventDetails;
using Template.Application.Features.Events.Queries.GetEventList;
using Template.Application.Responses;
using Xunit;

namespace GloboEvent.Api.IntegrationTest.Controllers.Events.Queries
{
    using static ApiRoutes.Event;
    public class EventController_QueriesTest : IntegrationTestBase
    {
        public string DefaultCategoryId { get; set; } = "B0788D2F-8003-43C1-92A4-EDC76A7C5DDE";

        public string DefaultEventId { get; set; } = "EE272F8B-6096-4CB6-8625-BB4BB2D89E8B";

        #region GetALL
        [Fact]
        public async Task GetAll_ReturnsAllSeededEvents()
        {
            await AuthenticateAsync();

            var response = await TestClient.GetAsync(GetAll);
            var content = await response.Content.ReadAsAsync<ApiResponse<EventListVm>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.DataList.Should().NotBeEmpty();
            content.DataList.Count().Should().Be(6);
        }

        [Fact]
        public async Task GetAll_WhenCreatedOne_ReturnsOneMore()
        {
            // //Arrange
            await AuthenticateAsync();
            var created = await TestClient.PostAsJsonAsync(
                Create,
                new CreateEventCommand { Name = "TestEvent", Price = 900, Date = DateTime.Now.AddDays(2), CategoryId = Guid.Parse(DefaultCategoryId) });

            // Act
            var response = await TestClient.GetAsync(GetAll);
            var content = await response.Content.ReadAsAsync<ApiResponse<EventListVm>>();
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.DataList.Should().NotBeEmpty();
            content.DataList.Count().Should().Be(7);
        }
        #endregion

        #region GetById
        [Fact]
        public async Task GetById_ShouldReturn_CorrectStatusAndEvent()
        {
            // //Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync(GetById.Replace("{id}", DefaultEventId));
            var content = await response.Content.ReadAsAsync<ApiResponse<EventDetailVm>>();
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.DataList.Should().BeNull();
            content.Data.Should().NotBeNull();

            content.Data.Name.Should().Be("John Egbert Live");
            content.Data.Category.Name.Should().Be("Concerts");
        }

        [Theory]
        [InlineData("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE", (int)HttpStatusCode.NotFound)]
        [InlineData("WrongType", (int)HttpStatusCode.BadRequest)]
        public async Task GetById_WithInvalidArgument_ShouldReturnsAppropriateStatusCode(string id, int res)
        {
            await AuthenticateAsync();

            var response = await TestClient.GetAsync(GetById.Replace("{id}", id));

            ((int)response.StatusCode).Should().Be(res);
        }
        #endregion
    }
}
