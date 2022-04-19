using System.Text.Json.Serialization;

namespace Template.Application.Features.Account
{
    public class AuthenticationResponse
    {
        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
        public string Token { get; set; }

        [JsonIgnore]
        public bool IsSuccess { get; set; } = true;
        [JsonIgnore]
        public string ErrorMessage { get; set; }
    }
}
