using Api.IntegrationTest.Base;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Template.API.Contract;
using Template.Application.Features.Categories;
using Template.Application.Features.Categories.Commands.Create;
using Template.Application.Features.Categories.Queries.GetCategoryWithEvent;
using Template.Application.Responses;
using Xunit;

namespace GloboEvent.Api.IntegrationTest.Controllers.Category.Query
{
    using static ApiRoutes.Category;
    public class CategoryController_QueriesTest : IntegrationTestBase
    {
        public string DefaultCategoryId { get; set; } = "B0788D2F-8003-43C1-92A4-EDC76A7C5DDE";
        public string WrongCategoryId { get; set; } = "62787623-4C52-43FE-B0C9-B7044FB5929B";
        #region GetALL
        [Fact]
        public async Task GetAll_ReturnsSeededResponse()
        {
            // //Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync(GetAll);
            var content = await response.Content.ReadAsAsync<ApiResponse<CategoryVm>>();
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.DataList.Should().NotBeEmpty();
            content.DataList.Count().Should().Be(4);
        }

        [Fact]
        public async Task GetAll_WhenCreatedOne_ReturnsOneMore()
        {
            // //Arrange
            await AuthenticateAsync();
            await CreateCategoryAsync(new CreateCategoryCommand { Name = "test" });

            // Act
            var response = await TestClient.GetAsync(GetAll);
            var content = await response.Content.ReadAsAsync<ApiResponse<CategoryVm>>();
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.DataList.Should().NotBeEmpty();
            content.DataList.Count().Should().Be(5);
        }

        #endregion

        #region GetById
        [Fact]
        public async Task GetByIdWithEvents_ReturnsCorrectCategory_AndCorrectEvents()
        {
            await AuthenticateAsync();
            List<string> expectedEvents = new List<string>() { "John Egbert Live", "The State of Affairs: Michael Live!", "Clash of the DJs", "Spanish guitar hits with Manuel" };

            var response = await TestClient.GetAsync(
                GetById
                .Replace("{id}", DefaultCategoryId)
                .Replace("{includeHistory}", true.ToString())
                );
            var content = await response.Content.ReadAsAsync<ApiResponse<CategoryWithEventsVm>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Data.Should().NotBeNull();
            content.Data.Name.Should().Be("Concerts");
            content.Data.Events.Count().Should().Be(4);
            var eventNames = content.Data.Events.Select(e => e.Name).ToList();
            expectedEvents.ForEach(ex => eventNames.Should().Contain(ex));
        }

        [Fact]
        public async Task GetByIdWithEvent_ReturnsOnlyTodayEvents_WhenAsked()
        {
            await AuthenticateAsync();

            var response = await TestClient.GetAsync(
                GetById
                .Replace("{id}", DefaultCategoryId)
                .Replace("{includeHistory}", false.ToString())
                );
            var content = await response.Content.ReadAsAsync<ApiResponse<CategoryWithEventsVm>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Data.Should().NotBeNull();
            content.Data.Events.Count().Should().Be(1);
            var eventDates = content.Data.Events.Select(e => e.Date).ToList();
            eventDates.ForEach(d => d.Date.Date.Should().Be(DateTime.Today.Date));
        }

        [Fact]
        public async Task GetById_WithWrongId_shouldReturn_NotFound()
        {
            await AuthenticateAsync();

            var response = await TestClient.GetAsync(
                GetById
                .Replace("{id}", WrongCategoryId)
                .Replace("{includeHistory}", false.ToString())
                );

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_WithWrongParameters_ShouldReturn_Badrequest()
        {
            await AuthenticateAsync();

            var response = await TestClient.GetAsync(
                GetById
                .Replace("{id}", "WrongData")
                .Replace("{includeHistory}", false.ToString())
                );
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

    }
}
