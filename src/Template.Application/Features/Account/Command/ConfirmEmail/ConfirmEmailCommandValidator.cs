using FluentValidation;

namespace Template.Application.Features.Account.Command.ConfirmEmail
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(c => c.Email)
                .EmailAddress()
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.RegistrationToken)
                .NotNull()
                .NotEmpty();
        }
    }
}
