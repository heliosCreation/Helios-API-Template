namespace Template.Application.Features.Account.Command.ResetPassword
{
    public class ResetPasswordCommand
    {
        public string Uid { get; set; }
        public string Email { get; set; }
        public string ResetToken { get; set; }
    }
}
