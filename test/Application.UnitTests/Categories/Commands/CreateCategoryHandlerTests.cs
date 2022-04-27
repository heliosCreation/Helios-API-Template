using Shouldly;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Template.Application;
using Template.Application.Features.Categories;
using Template.Application.Features.Categories.Commands.Create;
using Template.Application.Responses;
using Xunit;
using static UnitTests.Utils.DataSet.CategorySet;

namespace Application.UnitTests.Categories.Commands
{
    public class CreateCategoryHandlerTests : CategoryUnitTestBase
    {
        private readonly CreateCategoryCommandHandler _handler;
        private readonly CreateCategoryCommandValidator _validator;
        private readonly RequestHandlerHelper<CreateCategoryCommand, CreateCategoryCommandHandler, CreateCategoryCommandValidator, ApiResponse<CategoryVm>> _helper;
        public CreateCategoryHandlerTests()
        {
            _handler = new CreateCategoryCommandHandler(_mapper, _mockCategoryRepository.Object);
            _validator = new CreateCategoryCommandValidator(_mockCategoryRepository.Object);
            _helper = new RequestHandlerHelper<CreateCategoryCommand, CreateCategoryCommandHandler, CreateCategoryCommandValidator, ApiResponse<CategoryVm>>();
        }

        [Fact]
        public async Task Handle_CategoryWhenValid_IsAddedToRepo()
        {
            var command = new CreateCategoryCommand() { Name = "Test" };
            var result = await _helper.HandleRequest(command,_handler, _validator);

            var allCategories = await _mockCategoryRepository.Object.ListAllAsync();

            allCategories.Count.ShouldBe(3);
            result.Data.Name.ShouldBe("Test");
        }

        [Theory]
        [ClassData(typeof(CreateCategoryInvalidCommand))]
        public async Task Handle_CategoryWhenInValid_IsNotAddedToRepo_AndContainsErrorsInResponse(string name)
        {
            var command = new CreateCategoryCommand() { Name = name };
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            result.ErrorMessages.ShouldNotBeNull();

            var allCategories = await _mockCategoryRepository.Object.ListAllAsync();
            allCategories.Count.ShouldBe(2);

        }
    }
}
