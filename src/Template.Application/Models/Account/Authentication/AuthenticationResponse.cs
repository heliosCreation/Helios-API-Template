using System.Text.Json.Serialization;

namespace Template.Application.Model.Account.Authentification
{
    public class AuthenticationResponse
    {
        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
        public string Token { get; set; }
    }
}
