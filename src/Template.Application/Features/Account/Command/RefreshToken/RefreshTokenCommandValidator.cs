using FluentValidation;

namespace Template.Application.Features.Account.Command.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<ResfreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(c => c.RefreshToken)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.Token)
                .NotNull()
                .NotEmpty();
        }
    }
}
