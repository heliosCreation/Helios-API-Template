using Application.UnitTests.Mocks;
using AutoMapper;
using GloboEvent.Application.Features.Categories.Queries.GetCategoriesList;
using Moq;
using Shouldly;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;
using Template.Application.Features.Categories;
using Template.Application.Features.Categories.Queries.GetCategoriesList;
using Template.Application.Features.Categories.Queries.GetCategoryWithEvent;
using Template.Application.Profiles;
using Template.Application.Responses;
using UnitTests.Utils.DataSet;
using Xunit;

namespace Application.UnitTests.Categories.Queries
{
    using static CategorySet;

    public class CategoryQueryHandlersTest : CategoryUnitTestBase
    {

        [Fact]
        public async Task GetCategoryList_shouldReturns_EntireCollection_AndCorrectCast()
        {
            var handler = new GetCategoriesListQueryHandler(_mapper, _mockCategoryRepository.Object);
            var result = await handler.Handle(new GetCategoriesListQuery(), CancellationToken.None);

            result.ShouldBeOfType<ApiResponse<CategoryVm>>();

            result.DataList.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnDesiredEntity_AndCorrectCast_WhenValidIdProvided()
        {
            var handler = new GetCategoryWithEventsQueryHandler(_mapper, _mockCategoryRepository.Object);
            var result = await handler.Handle(new GetCategoryWithEventQuery() { IncludeHistory = true, Id = CategoryId1 }, CancellationToken.None);

            result.ShouldBeOfType<ApiResponse<CategoryWithEventsVm>>();

            result.Data.ShouldNotBeNull();
            result.Data.Name.ShouldBe(CategoryName1);
            result.Data.Events.Count.ShouldBe(2);
        }
        [Fact]
        public async Task GetCategoryById_ShouldReturnCategory_WithTodayEvents_WhenAskedTo()
        {
            var handler = new GetCategoryWithEventsQueryHandler(_mapper, _mockCategoryRepository.Object);
            var result = await handler.Handle(new GetCategoryWithEventQuery() { IncludeHistory = false, Id = CategoryId1 }, CancellationToken.None);


            result.ShouldBeOfType<ApiResponse<CategoryWithEventsVm>>();

            result.Data.ShouldNotBeNull();
            result.Data.Name.ShouldBe(CategoryName1);

            result.Data.Events.Count.ShouldBe(1);
            result.Data.Events.ToList().ForEach(e => e.Date.Date.ShouldBe(DateTime.Today.Date));
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnNotFound_WhenInvalidIdProvided()
        {
            var handler = new GetCategoryWithEventsQueryHandler(_mapper, _mockCategoryRepository.Object);
            var result = await handler.Handle(new GetCategoryWithEventQuery() { IncludeHistory = false, Id = Guid.NewGuid() }, CancellationToken.None);

            result.ShouldBeOfType<ApiResponse<CategoryWithEventsVm>>();
            result.Data.ShouldBeNull();
            result.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
        }
    }
}
