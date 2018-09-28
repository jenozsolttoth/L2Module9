using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Module7API.Services.Models;
using Responses;

namespace Module7API.Security.Services
{
    public class TokenBuilderService : ITokenBuilderService
    {
        private readonly IConfiguration _config;

        public TokenBuilderService(IConfiguration config)
        {
            _config = config;
        }

        public GenericResponse<string> BuildToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Birthdate", user.BirthDate.ToString(CultureInfo.InvariantCulture)),
                new Claim("Username", user.Username),
                new Claim("Email",user.Email)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);


            string resultToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new GenericResponse<string>(){Entity = resultToken, Message = "OK", StatusCode = 0, Success = true};
        }
    }
}
