using web_api.Data.Entities;

namespace web_api.Models
{
    public class LoginResponseModel
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
