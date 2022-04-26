using MediatR;
using Template.Application.Responses;

namespace Template.Application.Features.Account.Command.ResetPassword
{
    public class ResetPasswordCommand : IRequest<ApiResponse<object>>
    {
        public ResetPasswordCommand()
        {

        }
        public ResetPasswordCommand(string uid, string resetToken, string newPassword, string confirmationPassword)
        {
            Uid = uid;
            ResetToken = resetToken;
            NewPassword = newPassword;
            ConfirmationPassword = confirmationPassword;
        }

        public string Uid { get; set; }
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmationPassword { get; set; }
    }
}
