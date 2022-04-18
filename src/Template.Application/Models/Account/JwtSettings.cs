using System;

namespace Template.Application.Model.Account
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan TokenLifetime { get; set; }
    }
}
