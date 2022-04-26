using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Features.Categories.Commands.Delete;
using UnitTests.Utils.DataSet;
using Xunit;

namespace Application.UnitTests.Categories.Commands
{
    using static CategorySet;
    public class DeleteCategoryHandlerTest : CategoryUnitTestBase
    {
        private readonly DeleteCategoryCommandHandler _handler;

        public DeleteCategoryHandlerTest()
        {
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
