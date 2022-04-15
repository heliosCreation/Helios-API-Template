using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Categories.Queries.GetCategoriesList
{
    public class GetCategoriesListQuery : IRequest<ApiResponse<CategoryVm>>
    {
    }
}
