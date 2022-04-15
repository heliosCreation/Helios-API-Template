using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Categories.Commands.Create
{
    public class CreateCategoryCommand : IRequest<ApiResponse<CategoryVm>>
    {
        public string Name { get; set; }
    }
}
