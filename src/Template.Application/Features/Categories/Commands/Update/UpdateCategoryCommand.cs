using MediatR;
using System;
using Template.Application.Responses;

namespace Template.Application.Features.Categories.Commands.Update
{
    public class UpdateCategoryCommand : IRequest<ApiResponse<object>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

    }
}
