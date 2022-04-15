using MediatR;
using System;
using Template.Application.Responses;

namespace Template.Application.Features.Categories.Commands.Delete
{
    public class DeleteCategoryCommand : IRequest<ApiResponse<object>>
    {
        public Guid Id { get; set; }

        public DeleteCategoryCommand(Guid id)
        {
            Id = id;
        }
    }
}
