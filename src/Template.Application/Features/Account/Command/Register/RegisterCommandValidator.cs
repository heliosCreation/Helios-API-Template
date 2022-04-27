using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Contracts.Identity;

namespace Template.Application.Features.Account.Command.Register
{

    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IAuthenticationService _authenticationService;

        public RegisterCommandValidator(IAuthenticationService categoryRepository)
        {
            _authenticationService = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));

            RuleFor(c => c.FirstName)
                .NotEmpty()
                .NotNull()
                .MaximumLength(120);

            RuleFor(c => c.LastName)
                .NotEmpty()
                .NotNull()
                .MaximumLength(120);

            RuleFor(c => c.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress()
                .MaximumLength(120);


            RuleFor(c => c.UserName)
                .NotEmpty()
                .NotNull()
                .MaximumLength(120);

            RuleFor(c => c.Password)
                .NotEmpty()
                .NotNull()
                .MaximumLength(120);

            RuleFor(c => c.ConfirmationPassword)
                .Equal(c => c.Password);

            RuleFor(e => e)
            .MustAsync(IsNameUnique).WithMessage("Username already taken.")
            .MustAsync(IsEmailUnique).WithMessage("Email already taken.");

        }

        private async Task<bool> IsNameUnique(RegisterCommand e, CancellationToken c)
        {
            return !await _authenticationService.UsernameExist(e.UserName);
        }

        private async Task<bool> IsEmailUnique(RegisterCommand e, CancellationToken c)
        {
            return !await _authenticationService.UserEmailExist(e.Email);
        }
    }
}
