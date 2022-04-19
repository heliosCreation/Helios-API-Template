using System.Collections.Generic;

namespace Template.Application.Features.Account.Command.RegistrationToken
{
    public class RegistrationTokenResponse
    {
        public RegistrationTokenResponse()
        {

        }

        public RegistrationTokenResponse(string error)
        {
            Error = error;
            IsSuccess = false;
        }
        public string Token { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Error { get; set; }
    }
}
