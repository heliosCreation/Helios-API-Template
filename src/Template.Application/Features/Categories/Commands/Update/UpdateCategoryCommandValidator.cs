using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;

namespace Template.Application.Features.Categories.Commands.Update
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));

            RuleFor(p => p.Name)
                .NotNull()
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(100).WithMessage("{PropertyName} can't exceed 100 characters.");

            RuleFor(e => e)
                .MustAsync(IsNameUniqueForUpdate).WithMessage("A category with the same given name already exists.");
        }

        private async Task<bool> IsNameUniqueForUpdate(UpdateCategoryCommand e, CancellationToken c)
        {
            return await _categoryRepository.IsNameUniqueForUpdate(e.Id, e.Name);
        }
    }
}
