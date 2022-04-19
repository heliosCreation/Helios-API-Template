namespace Template.Application.Features.Account.Command.ForgotPassword
{
    public class ForgotPasswordResponse
    {
        public ForgotPasswordResponse(string uid, string token)
        {
            UserId = uid;
            ResetPasswordToken = token;
        }
        public string UserId { get; set; }
        public string ResetPasswordToken { get; set; }
    }
}
