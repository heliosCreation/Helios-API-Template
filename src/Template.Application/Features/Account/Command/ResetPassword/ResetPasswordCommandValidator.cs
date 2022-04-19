using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Application.Features.Account.Command.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(c => c.Uid)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.ResetToken)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.NewPassword)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.ConfirmationPassword)
                .Equal(c => c.NewPassword);
        }
    }
}
