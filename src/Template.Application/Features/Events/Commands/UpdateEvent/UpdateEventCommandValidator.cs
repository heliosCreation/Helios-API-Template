using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;
using Template.Domain.Entities;

namespace Template.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAsyncRepository<Category> _categoryRepository;

        public UpdateEventCommandValidator(IEventRepository eventRepository, IAsyncRepository<Category> categoryRepository)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));

            RuleFor(p => p.Name)
                .NotNull()
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(50).WithMessage("{PropertyName} can't exceed 50 characters");

            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("{PropertyName} can't exceed 500 characters.");

            RuleFor(p => p.Date)
                .Must(p => p > DateTime.UtcNow).WithMessage("{PropertyName} must be set in the future.")
                .NotNull()
                .NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Price)
                .NotNull()
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0);

            RuleFor(e => e)
                .MustAsync(AreNameAndDateuniqueForUpdate).WithMessage("An event with the same name and date already exist.")
                .MustAsync(CategoryExists).WithMessage("Category with this {PropertyName} was not found");
        }

        private async Task<bool> AreNameAndDateuniqueForUpdate(UpdateEventCommand e, CancellationToken c)
        {
            return await _eventRepository.IsUniqueNameAndDateForUpdate(e.Name, e.Date, e.Id);
        }

        private async Task<bool> CategoryExists(UpdateEventCommand e, CancellationToken c)
        {
            return await _categoryRepository.GetByIdAsync(e.CategoryId) != null;
        }
    }
}
