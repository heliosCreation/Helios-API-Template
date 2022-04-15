using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contrats.Persistence;
using Template.Domain.Entities;

namespace Template.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAsyncRepository<Category> _categoryRepository;

        public CreateEventCommandValidator(IEventRepository eventRepository, IAsyncRepository<Category> categoryRepository)
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
                .Must(p => p >= DateTime.UtcNow).WithMessage("{PropertyName} must not be set in the past.")
                .NotNull()
                .NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Price)
                .NotNull()
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(-1);

            RuleFor(p => p.CategoryId)
                .NotNull().WithMessage("{PropertyName} is required");

            RuleFor(e => e)
                .MustAsync(AreNameAndDateunique).WithMessage("An event with the same name and date already exist.")
                .MustAsync(CategoryExists).WithMessage("Category with this {PropertyName} was not found");

        }

        private async Task<bool> AreNameAndDateunique(CreateEventCommand e, CancellationToken c)
        {
            return await _eventRepository.IsUniqueNameAndDate(e.Name, e.Date);
        }
        private async Task<bool> CategoryExists(CreateEventCommand e, CancellationToken c)
        {
            return await _categoryRepository.GetByIdAsync(e.CategoryId) != null;
        }
    }
}
