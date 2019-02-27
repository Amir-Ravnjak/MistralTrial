using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace MistralTrialAPI.Controllers
{
    public class TokenController : Controller
    {
        private IConfiguration Configuration;
        public TokenController(IConfiguration config)
        {
            Configuration = config;
        }
        public IActionResult CreateToken([FromHeader]string username,[FromHeader] string password)
        {

            IActionResult response = Unauthorized();
            if (username.Equals("admin") && password.Equals("admin"))
            {
                var jwttoken = JwtTokenBuilder();

                response = Ok(jwttoken);
            }

            return response;
        }

        private string JwtTokenBuilder()
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(Configuration["JWT:key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(issuer: Configuration["JWT:issuer"],
                audience: Configuration["JWT:audience"], signingCredentials: credentials,
                expires: DateTime.Now.AddMinutes(10)
                );



            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}