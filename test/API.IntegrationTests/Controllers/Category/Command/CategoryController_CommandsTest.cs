using Api.IntegrationTest.Base;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Template.API.Contract;
using Template.Application.Features.Categories;
using Template.Application.Features.Categories.Commands.Create;
using Template.Application.Features.Categories.Commands.Update;
using Template.Application.Features.Categories.Queries.GetCategoryWithEvent;
using Template.Application.Responses;
using Xunit;

namespace Api.IntegrationTest.Controllers.Category.Command
{
    using static ApiRoutes.Category;
    using static Api.IntegrationTests.Utils.AccountTools;
    using static Api.IntegrationTests.Utils.CategoryTools;

    public class CategoryController_CommandsTest : IntegrationTestBase
    {
        public string DefaultCategoryId { get; set; } = "B0788D2F-8003-43C1-92A4-EDC76A7C5DDE";
        public string WrongCategoryId { get; set; } = "62787623-4C52-43FE-B0C9-B7044FB5929B";

        #region Post
        [Fact]
        public async Task AddCategory_WhenValid_returnCorrectDataAndStatusCode()
        {
            await AuthenticateAsync();
            var categoryName = "Test";

            var response = await TestClient.PostAsJsonAsync(Create, new CreateCategoryCommand { Name = categoryName });
            var content = await response.Content.ReadAsAsync<ApiResponse<CategoryVm>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Data.Name.Should().Be(categoryName);
        }

        [Theory]
        [ClassData(typeof(CreateCategoryInvalidCommand))]
        public async Task AddCategory_WithInvalidData_returnBadRequest(string data)
        {
            await AuthenticateAsync();

            var response = await TestClient.PostAsJsonAsync(Create, new CreateCategoryCommand { Name = data });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Put
        [Fact]
        public async Task UpdateCategory_WithValidData_ReturnsOkStatusCode_AndUpdatesEntity()
        {
            await AuthenticateAsync();
            var updatedName = "Updated";

            var response = await TestClient.PutAsJsonAsync(Update.Replace("{id}", DefaultCategoryId), new UpdateCategoryCommand { Id = Guid.Parse(DefaultCategoryId), Name = updatedName });
            var Getresponse = await TestClient.GetAsync(
                    GetById
                    .Replace("{id}", DefaultCategoryId)
                    .Replace("{includeHistory}", true.ToString())
                    );
            var content = await Getresponse.Content.ReadAsAsync<ApiResponse<CategoryWithEventsVm>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Data.Name.Should().Be(updatedName);
        }
        [Theory]
        [ClassData(typeof(UpdateCategoryInvalidCommand))]
        public async Task UpdateCategory_WithInValidData_AppropriateStatusCode(Guid id, string data, int status)
        {
            await AuthenticateAsync();

            var response = await TestClient.PutAsJsonAsync(
                Update.Replace("{id}", id.ToString()),
                new UpdateCategoryCommand { Id = id, Name = data });

            ((int)response.StatusCode).Should().Be(status);
        }

        [Fact]
        public async Task UpdateCategory_NonRegisteredId_ReturnsNotFound()
        {
            await AuthenticateAsync();

            var response = await TestClient.PutAsJsonAsync(
                Update.Replace("{id}", WrongCategoryId),
                new UpdateCategoryCommand { Id = Guid.Parse(WrongCategoryId), Name = "Test" });
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        #endregion

        #region Delete
        [Theory]
        [ClassData(typeof(DeleteCategoryCommandData))]
        public async Task Should_returnsValidStatusCode_DependingOnFoundOrNot(string id, int result)
        {
            await AuthenticateAsync();

            var response = await TestClient.DeleteAsync(Delete.Replace("{id}", id));

            ((int)response.StatusCode).Should().Be(result);

        }

        [Fact]
        public async Task Should_returnsNotFound_afterDeletingCategory()
        {
            await AuthenticateAsync();

            await TestClient.DeleteAsync(Delete.Replace("{id}", DefaultCategoryId));
            var response = await TestClient.GetAsync(
                GetById
                .Replace("{id}", DefaultCategoryId)
                .Replace("{includeHistory}", true.ToString())
                );
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }
        #endregion
    }
}
