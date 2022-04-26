using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Template.Application;
using Template.Application.Features.Events.Commands.UpdateEvent;
using Template.Application.Responses;
using Xunit;
using static UnitTests.Utils.DataSet.EventSet;

namespace Application.UnitTests.Events.Commands
{
    public class UpdateEventHandlerTests : EventUnitTestBase
    {
        private readonly UpdateEventCommandHandler _handler;
        private readonly UpdateEventCommandValidator _validator;
        private readonly RequestHandlerHelper<UpdateEventCommand, UpdateEventCommandHandler, UpdateEventCommandValidator, ApiResponse<object>> _helper;

        public UpdateEventHandlerTests()
        {
            _handler = new UpdateEventCommandHandler(_mapper, _mockEventRepository.Object);
            _validator = new UpdateEventCommandValidator(_mockEventRepository.Object, _mockCategoryRepository.Object);
            _helper = new RequestHandlerHelper<UpdateEventCommand, UpdateEventCommandHandler, UpdateEventCommandValidator, ApiResponse<object>>();
        }

        [Fact]
        public async Task Handle_EventWhenValid_UpdatesRepo_AndReturnsOkStatusCode()
        {
            var command = new UpdateEventCommand
            {
                Id = EventId1,
                Name = NewEvent.Name,
                Date = NewEvent.Date,
                Price = NewEvent.Price,
                CategoryId = NewEvent.CategoryId
            };

            var result = _helper.HandleRequest(command, _handler, _validator);

            var target = await _mockEventRepository.Object.GetByIdAsync(EventId1);
            target.Name.ShouldBe(NewEvent.Name);
            target.Date.ShouldBe(NewEvent.Date);
            target.Price.ShouldBe(NewEvent.Price);
            target.CategoryId.ShouldBe(NewEvent.CategoryId);
        }

        [Theory]
        [ClassData(typeof(UpdateEventInvalidCommand))]
        public async Task Handle_EventWhenInvalid_DoesNotUpdatesRepo_AndReturnsBadRequest(Guid id, string name, int price, DateTime date, Guid categoryId)
        {
            var command = new UpdateEventCommand
            {
                Id = id,
                Name = name,
                Date = date,
                Price = price,
                CategoryId = categoryId
            };

            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);

            var target = await _mockEventRepository.Object.GetByIdAsync(EventId1);
            target.Name.ShouldBe(EventName1);
        }

        [Fact]
        public async Task Handle_EventWhenInvalidIdProvided_DoesNotUpdatesRepo_AnReturnsNotFound()
        {
            var command = new UpdateEventCommand
            {
                Id = Guid.NewGuid(),
                Name = NewEvent.Name,
                Date = NewEvent.Date,
                Price = NewEvent.Price,
                CategoryId = NewEvent.CategoryId
            };

            var result = await _helper.HandleRequest(command, _handler, _validator);
            var target = await _mockEventRepository.Object.GetByIdAsync(EventId1);


            result.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
            target.Name.ShouldBe(EventName1);
        }
    }

}
