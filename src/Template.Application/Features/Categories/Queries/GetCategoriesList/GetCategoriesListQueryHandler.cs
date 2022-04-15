using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;
using Template.Application.Features.Categories;
using Template.Application.Features.Categories.Queries.GetCategoriesList;
using Template.Application.Responses;
using Template.Domain.Entities;

namespace GloboEvent.Application.Features.Categories.Queries.GetCategoriesList
{
    public class GetCategoriesListQueryHandler : IRequestHandler<GetCategoriesListQuery, ApiResponse<CategoryVm>>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Category> _categoryRepository;

        public GetCategoriesListQueryHandler(
            IMapper mapper,
            IAsyncRepository<Category> categoryRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<ApiResponse<CategoryVm>> Handle(GetCategoriesListQuery request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<CategoryVm>();
            var allCategories = (await _categoryRepository.ListAllAsync()).OrderBy(x => x.Name);
            response.DataList = _mapper.Map<List<CategoryVm>>(allCategories);
            return response;
        }
    }

}
