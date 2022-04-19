using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Template.Application.Features.Account.Command.Register
{
    public class RegistrationResponse
    {
        public RegistrationResponse()
        {

        }

        public RegistrationResponse(string id)
        {
            UserId = id;
        }

        public RegistrationResponse(List<string> errors)
        {
            Succeeded = false;
            Errors = errors;
        }

        [JsonIgnore]
        public bool Succeeded { get; protected set; } = true;
        public string CallBackUrl { get; set; }

        [JsonIgnore]
        public IEnumerable<string> Errors { get; } = new List<string>();
        public string? UserId { get; set; }
    }

}
