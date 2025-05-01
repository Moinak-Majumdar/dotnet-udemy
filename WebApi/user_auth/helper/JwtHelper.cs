using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using user_auth.models;

namespace user_auth.helper
{
    public class JwtHelper(IConfiguration config)
    {
        public string CreateJwtToken(User u)
        {
            Claim[] claims = [
                new Claim("UserId", u.UserId.ToString()),
                new Claim("FirstName", u.FirstName),
                new Claim("LastName", u.LastName),
                new Claim("Email", u.Email)
            ];

            JWtSettings? jwtSettings = config.GetSection("Jwt").Get<JWtSettings>();
            SymmetricSecurityKey tokenKey = new(Encoding.UTF8.GetBytes(jwtSettings?.TokenKey ?? ""));

            SigningCredentials credentials = new(tokenKey, SecurityAlgorithms.HmacSha512Signature);

            DateTime now = DateTime.Now;

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                NotBefore = now,
                IssuedAt = now,
                Expires = now.AddMinutes(jwtSettings?.ExpInMin ?? 60),
            };

            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}