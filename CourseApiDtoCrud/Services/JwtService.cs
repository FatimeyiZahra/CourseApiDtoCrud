using CourseApiDtoCrud.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Services
{
    public interface IJwtService
    {
        public string Generate(IList<string> roles, AppUser user);
    }
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Generate(IList<string> roles, AppUser user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim("FullName", user.FullName));

            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList());

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:secret").Value));
            SigningCredentials creds = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new JwtSecurityToken
                (
                    signingCredentials: creds,
                    claims: claims,
                    issuer: _configuration.GetSection("JWT:issuer").Value,
                    audience: _configuration.GetSection("JWT:audience").Value,
                    expires: DateTime.UtcNow.AddDays(3)
                );

            string tokenStr = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return tokenStr;
        }
    }

}
