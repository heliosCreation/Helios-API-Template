using System.Collections.Generic;

namespace Template.Application.Model.Account
{
    public class CustomIdentityResult
    {
        CustomIdentityResult()
        {

        }

        public CustomIdentityResult(string id)
        {
            UserId = id;
        }

        public CustomIdentityResult(List<string> errors)
        {
            Succeeded = false;
            Errors = errors;
        }
        public bool Succeeded { get; protected set; } = true;
        public IEnumerable<string> Errors { get; } = new List<string>();
        public string? UserId { get; set; }
    }
}
