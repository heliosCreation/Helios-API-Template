using FluentValidation;

namespace Template.Application.Features.Account.Command.RegistrationToken
{
    public class RegistrationTokenCommandValidator : AbstractValidator<RegistrationTokenCommand>
    {
        public RegistrationTokenCommandValidator()
        {
            RuleFor(c => c.Uid)
                .NotNull()
                .NotEmpty();
        }
    }
}
