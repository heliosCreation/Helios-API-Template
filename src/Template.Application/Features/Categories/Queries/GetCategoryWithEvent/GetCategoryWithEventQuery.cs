using MediatR;
using System;
using Template.Application.Responses;

namespace Template.Application.Features.Categories.Queries.GetCategoryWithEvent
{
    public class GetCategoryWithEventQuery : IRequest<ApiResponse<CategoryWithEventsVm>>
    {
        public Guid Id { get; set; }
        public bool IncludeHistory { get; set; }
    }
}
