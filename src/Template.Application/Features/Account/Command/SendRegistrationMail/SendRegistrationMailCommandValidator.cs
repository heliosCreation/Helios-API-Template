using FluentValidation;

namespace Template.Application.Features.Account.Command.SendRegistrationMail
{
    public class SendRegistrationMailCommandValidator : AbstractValidator<SendRegistrationMailCommand>
    {
        public SendRegistrationMailCommandValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();

            RuleFor(c => c.CallBackUrl)
                .NotEmpty()
                .NotNull();
        }
    }
}
