using Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;
using Template.Application.Features.Categories.Commands.Delete;
using UnitTests.Utils.DataSet;
using Xunit;

namespace GloboEvent.Application.UnitTests.Categories.Commands
{
    using static CategorySet;
    public class DeleteCategoryHandlerTest
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly DeleteCategoryCommandHandler _handler;

        public DeleteCategoryHandlerTest()
        {
            _mockCategoryRepository = new MockCategoryRepository().GetEntityRepository();
            _handler = new DeleteCategoryCommandHandler(_mockCategoryRepository.Object);


        }


        [Theory]
        [ClassData(typeof(DeleteCategoryCommandData))]
        public async Task Handle_DeleteCategory_ShouldReturns_AppropriateValues(Guid id, int statusCode)
        {
            var command = new DeleteCategoryCommand(id);
            var result = await _handler.Handle(command, CancellationToken.None);

            result.StatusCode.ShouldBe(statusCode);
        }
    }
}
