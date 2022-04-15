using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;

namespace Template.Application.Features.Categories.Commands.Create
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandValidator(ICategoryRepository authenticationService)
        {
            _categoryRepository = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));

            RuleFor(p => p.Name)
                .NotNull()
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(100).WithMessage("{PropertyName} can't exceed 100 characters.");

            RuleFor(e => e)
                .MustAsync(IsNameUnique).WithMessage("A category with the same given name already exists.");
        }

        private async Task<bool> IsNameUnique(CreateCategoryCommand e, CancellationToken c)
        {
            return await _categoryRepository.IsNameUnique(e.Name);
        }
    }
}
