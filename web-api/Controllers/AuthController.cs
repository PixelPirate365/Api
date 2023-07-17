using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using web_api.Data;
using web_api.Data.Entities;
using web_api.Extensions;
using web_api.Models;

namespace web_api.Controllers {
    [ApiController]
    [Route("auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase {
        readonly MyContext _context;

        public const string TOKEN_SECRET = "This is a scret token !!";

        public AuthController(MyContext context) {
            _context = context;
        }
        /// <summary>
        ///     Authentication login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model) {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (user == null || user.Pwd != model.Pwd.Hash())
                return Unauthorized();

            var token = generateToken(user);

            return new LoginResponseModel {
                User = user,
                Token = token
            };
        }


        private string generateToken(User user) {
            var claims = new List<Claim> {
            new Claim("id", user.Id.ToString())
        };

            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TOKEN_SECRET));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "web-api",
                //audience: "orTheKing.com",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(30),

                signingCredentials: credentials
           );
            string ret = new JwtSecurityTokenHandler()
                .WriteToken(token);
            return ret;
        }
    }
}
