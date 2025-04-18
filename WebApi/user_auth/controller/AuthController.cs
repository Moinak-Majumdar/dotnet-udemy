using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using user_auth.context;
using user_auth.Dto;
using user_auth.models;

namespace user_auth.controller
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IConfiguration config) : ControllerBase
    {
        private readonly DapperContext _dapper = new(config);

        [HttpPost("Register")]
        public async Task<IActionResult> UserRegister(UserRegisterDto ur)
        {
            if (ur.Password != ur.PasswordConfirm)
            {
                throw new Exception("Password do not match");
            }

            // User existingUser = _dapper.LoadDataSingle<User>("SELECT * from TblUsers where Email = '" + ur.Email + "'");

            // if (existingUser.UserId > 0)
            // {
            //     throw new Exception("Email id is already in used");
            // }

            byte[] passwordSalt = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            string paper = config.GetSection("AppSettings:PasswordKey").Value ?? "";
            string passwordWithPaper = ur.Password + paper;

            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: passwordWithPaper,
                salt: passwordSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000000,
                numBytesRequested: 32
            );

            string sql = @"
                DECLARE @UserId BIGINT;

                INSERT INTO TblUsers (FirstName, LastName, Email, Gender, IsActive, IsPasswordSet)
                VALUES (@FirstName, @LastName, @Email, @Gender, @IsActive, @IsPasswordSet);

                SET @UserId = SCOPE_IDENTITY();

                INSERT INTO TblUserPassword (UserId, PasswordHash, PasswordSalt)
                VALUES(@UserId, @PasswordHash, @PasswordSalt)
            ";

            object param = new
            {
                ur.FirstName,
                ur.LastName,
                ur.Email,
                ur.Gender,
                IsActive = true,
                IsPasswordSet = true,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            bool status = await _dapper.ExecuteWithParams(sql, param);
            if (status)
            {
                return Ok();
            }
            throw new Exception("Failed to add user");
        }

    }
}