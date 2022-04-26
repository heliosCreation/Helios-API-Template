using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Template.Application.Features.Events.Commands.CreateEvent;
using Template.Application.Responses;
using Xunit;
using static UnitTests.Utils.DataSet.EventSet;

namespace Application.UnitTests.Events.Commands
{
    public class CreateEventHandlerTests : EventUnitTestBase
    {
        private readonly CreateEventCommandHandler _handler;
        private readonly CreateEventCommandValidator _validator;
        private readonly RequestHandlerHelper<CreateEventCommand, CreateEventCommandHandler, CreateEventCommandValidator, ApiResponse<CreateEventResponse>> _helper;
        public CreateEventHandlerTests()
        {
            _handler = new CreateEventCommandHandler(_mapper, _mockEventRepository.Object);
            _validator = new CreateEventCommandValidator(_mockEventRepository.Object, _mockCategoryRepository.Object);
            _helper = new RequestHandlerHelper<CreateEventCommand, CreateEventCommandHandler, CreateEventCommandValidator, ApiResponse<CreateEventResponse>>();
        }

        [Fact]
        public async Task Handle_EventWhenValid_ReturnCorrectStatusAndData_AndIsAddedToRepo()
        {
            var command = new CreateEventCommand()
            {
                Name = NewEvent.Name,
                Date = NewEvent.Date,
                Price = NewEvent.Price,
                CategoryId = NewEvent.CategoryId
            };

            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Data.ShouldNotBeNull();
            result.Data.ShouldBeOfType<ApiResponse<CreateEventResponse>>();

            var allEvents = await _mockEventRepository.Object.ListAllAsync();
            allEvents.Count.ShouldBe(3);
        }

        [Theory]
        [ClassData(typeof(CreateEventInvalidCommand))]
        public async Task Handle_EventWhenInvalid_IsNotAddedToRepo_AndReturnsBadRequestInResponse(string name, int price, DateTime date, Guid id)
        {
            var command = new CreateEventCommand()
            {
                Name = name,
                Date = date,
                Price = price,
                CategoryId = id
            };

            var result = await _helper.HandleRequest(command, _handler, _validator);

            result.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            result.Data.ShouldBeNull();

            var allEvents = await _mockEventRepository.Object.ListAllAsync();
            allEvents.Count.ShouldBe(2);
        }
    }
}
