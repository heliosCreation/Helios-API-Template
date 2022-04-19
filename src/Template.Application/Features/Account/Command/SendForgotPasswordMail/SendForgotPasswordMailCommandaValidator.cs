using FluentValidation;

namespace Template.Application.Features.Account.Command.SendForgotPasswordMail
{
    public class SendForgotPasswordMailCommandaValidator : AbstractValidator<SendForgotPasswordMailCommand>
    {
        public SendForgotPasswordMailCommandaValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();

            RuleFor(c => c.CallbackLink)
                .NotEmpty()
                .NotNull();
        }
    }
}
