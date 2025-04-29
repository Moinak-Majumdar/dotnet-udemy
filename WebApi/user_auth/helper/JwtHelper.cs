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
            SymmetricSecurityKey tokenKey = new(Encoding.UTF8.GetBytes(jwtSettings != null ? jwtSettings.TokenKey : ""));

            DateTime expire = DateTime.Now.AddMinutes(jwtSettings != null ? jwtSettings.ExpInMin : 60);

            SigningCredentials credentials = new(tokenKey, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = expire
            };

            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}