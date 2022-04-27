namespace Template.Application.Features.Account.Command.ResetPassword
{
    public class ResetPasswordResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string ErrorMessage { get; set; }
        public ResetPasswordResponse()
        {

        }
        public ResetPasswordResponse(string error)
        {
            IsSuccess = false;
            ErrorMessage = error;
        }
    }
}
