using Application.UnitTests.Mocks;
using AutoMapper;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Template.Application;
using Template.Application.Contrats.Persistence;
using Template.Application.Features.Categories.Commands.Update;
using Template.Application.Features.Categories.Queries.GetCategoryWithEvent;
using Template.Application.Profiles;
using Template.Application.Responses;
using Xunit;
using static UnitTests.Utils.DataSet.CategorySet;

namespace Application.UnitTests.Categories.Commands
{
    public class UpdateCategoryHandlerTest : CategoryUnitTestBase
    {
        private readonly UpdateCategoryCommandHandler _handler;
        private readonly UpdateCategoryCommandValidator _validator;
        private readonly RequestHandlerHelper<UpdateCategoryCommand, UpdateCategoryCommandHandler, UpdateCategoryCommandValidator, ApiResponse<object>> _helper;

        public UpdateCategoryHandlerTest()
        {
            _handler = new UpdateCategoryCommandHandler(_mockCategoryRepository.Object, _mapper);
            _validator = new UpdateCategoryCommandValidator(_mockCategoryRepository.Object);
            _helper = new RequestHandlerHelper<UpdateCategoryCommand, UpdateCategoryCommandHandler, UpdateCategoryCommandValidator, ApiResponse<object>>();
        }

        [Fact]
        public async Task Handle_UpdateCategory_WhenValid_UpdatesAndReturnCorrectApiResponse()
        {
            var command = new UpdateCategoryCommand() { Name = "Test", Id = CategoryId1 };
            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var Gethandler = new GetCategoryWithEventsQueryHandler(_mapper, _mockCategoryRepository.Object);

            var GetResult = await Gethandler.Handle(new GetCategoryWithEventQuery() { IncludeHistory = false, Id = CategoryId1 }, CancellationToken.None);
            GetResult.Data.Name.ShouldBe("Test");
        }

        [Theory]
        [ClassData(typeof(UpdateCategoryInvalidCommand))]
        public async Task Handle_UpdateCategory_WhenInvalidDataArePassed_ReturnsAppropriateResponse_AndDoesNotUpdate(Guid id, string data, int status)
        {
            var command = new UpdateCategoryCommand() { Name = data, Id = id };
            var result = await _helper.HandleRequest(command, _handler, _validator);


            result.StatusCode.ShouldBe(status);
            var Gethandler = new GetCategoryWithEventsQueryHandler(_mapper, _mockCategoryRepository.Object);

            var GetResult = await Gethandler.Handle(new GetCategoryWithEventQuery() { IncludeHistory = false, Id = CategoryId1 }, CancellationToken.None);
            GetResult.Data.Name.ShouldBe(CategoryName1);
        }
    }
}
