using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;
using Template.Application.Responses;

namespace Template.Application.Features.Categories.Commands.Delete
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResponse<object>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }
        public async Task<ApiResponse<object>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<object>();
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
            {
                return response.setNotFoundResponse($"Category with Id {request.Id} not Found");
            }

            await _categoryRepository.DeleteAsync(category);
            return response;
        }
    }
}
