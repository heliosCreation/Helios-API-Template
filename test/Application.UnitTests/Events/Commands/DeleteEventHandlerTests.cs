using Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;
using Template.Application.Features.Events.Commands.DeleteEvent;
using Xunit;
using static UnitTests.Utils.DataSet.EventSet;

namespace Application.UnitTests.Events.Commands
{
    public class DeleteEventHandlerTests : EventUnitTestBase
    {
        private readonly DeleteEventCommandHandler _handler;
        public DeleteEventHandlerTests()
        {
            _handler = new DeleteEventCommandHandler(_mockEventRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldDelete_WhenValidIdProvided()
        {
            var command = new DeleteEventCommand(EventId1);

            await _handler.Handle(command, CancellationToken.None);

            var target = await _mockEventRepository.Object.GetByIdAsync(EventId1);
            target.ShouldBeNull();
        }

        [Fact]
        public async Task Handle_ShouldNotDelete_WhenInvalidIdProvided_AndReturnsNotFoundApiResponse()
        {
            var command = new DeleteEventCommand(Guid.NewGuid());

            var result = await _handler.Handle(command, CancellationToken.None);
            var target = await _mockEventRepository.Object.GetByIdAsync(EventId1);

            result.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
            target.ShouldNotBeNull();
        }
    }
}
