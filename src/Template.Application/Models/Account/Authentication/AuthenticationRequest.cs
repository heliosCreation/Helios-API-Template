using System.ComponentModel.DataAnnotations;

namespace Template.Application.Model.Account.Authentification
{
    public class AuthenticationRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(120)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(120)]
        public string Password { get; set; }
        

    }
}
