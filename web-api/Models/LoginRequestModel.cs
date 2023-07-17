using System.ComponentModel.DataAnnotations;

namespace web_api.Models
{
    public class LoginRequestModel {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(4)]
        public string Pwd { get; set; }
    }
}
