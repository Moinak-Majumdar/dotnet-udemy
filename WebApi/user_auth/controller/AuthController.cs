using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using user_auth.context;
using user_auth.Dto;
using user_auth.helper;
using user_auth.models;

namespace user_auth.controller
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IConfiguration config) : ControllerBase
    {
        private readonly DapperContext _dapper = new(config);
        private readonly PasswordHelper _ph = new(config);
        private readonly JwtHelper _jwtHelper = new(config);


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> UserRegister(UserRegisterDto ur)
        {
            if (ur.Password != ur.PasswordConfirm)
            {
                return BadRequest(new { errorMessage = "Password did not matched." });
            }

            User? existingUser = await _dapper.LoadDataSingle<User>("SELECT * from TblUsers where Email = '" + ur.Email + "'");

            if (existingUser != null)
            {
                return BadRequest(new { errorMessage = "Email is already in use." });
            }

            byte[] passwordSalt = new byte[16];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetNonZeroBytes(passwordSalt);

            byte[] passwordHash = _ph.GetPasswordHash(salt: passwordSalt, password: ur.Password);

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
                return Ok(new { message = "User created successfully." });
            }
            return BadRequest(new { errorMessage = "Failed to create user." });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin(UserLoginDto ul)
        {
            string sql = @"
                SELECT 
                u.UserId, u.Email, u.IsActive, u.FirstName, U.LastName, u.IsPasswordSet,
                up.PasswordHash, up.PasswordSalt 
                From TblUsers as u
                Left Join TblUserPassword as up on u.UserId = up.UserId
                WHERE u.Email = '" + ul.Email + "' " +
            "AND u.IsDeleted = 0";

            UserLoginConfirmationDto? dbUser = await _dapper.LoadDataSingle<UserLoginConfirmationDto>(sql);

            if (dbUser == null)
            {
                return BadRequest(new { errorMessage = "No user found with requested email." });
            }
            else if (!dbUser.IsPasswordSet)
            {
                return BadRequest(new { errorMessage = "Your password is not set yet." });
            }
            else if (!dbUser.IsActive)
            {
                return BadRequest(new { errorMessage = "Your account is currently deactivated." });
            }
            else
            {
                byte[] passwordHash = _ph.GetPasswordHash(salt: dbUser.PasswordSalt, password: ul.Password);

                if (passwordHash.SequenceEqual(dbUser.PasswordHash))
                {
                    User user = new()
                    {
                        UserId = dbUser.UserId,
                        FirstName = dbUser.FirstName,
                        LastName = dbUser.LastName,
                        Email = dbUser.Email
                    };
                    string token = _jwtHelper.CreateJwtToken(user);
                    return Ok(new
                    {
                        message = "Welcome " + dbUser.FirstName + " " + dbUser.LastName + "!!",
                        token
                    });
                }
                else
                {
                    return Unauthorized(new { errorMessage = "Incorrect password" });
                }
            }
        }


        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(UserLoginDto ul)
        {

            User? dbUser = await _dapper.LoadDataSingle<User>("Select * from TblUsers where Email = '" + ul.Email + "'");

            if (dbUser == null)
            {
                return BadRequest(new { errorMessage = "No user found with requested email." });
            }
            else
            {
                byte[] passwordSalt = new byte[16];
                using RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetNonZeroBytes(passwordSalt);
                byte[] hash = _ph.GetPasswordHash(passwordSalt, ul.Password);

                string sql = @"
                    update tblUsers set IsPasswordSet = 1, IsActive = 1
                    where UserId = @UserId

                    IF exists (select 1 from TblUserPassword where UserId = @UserId)
                    begin
                        update TblUserPassword set 
                            PasswordHash = @PasswordHash,
                            PasswordSalt = @PasswordSalt
                        Where UserId = @UserId
                    end
                    Else
                    begin
                        INSERT INTO TblUserPassword (UserId, PasswordHash, PasswordSalt)
                        VALUES(@UserId, @PasswordHash, @PasswordSalt)
                    end
                ";
                object param = new
                {
                    dbUser.UserId,
                    PasswordHash = hash,
                    PasswordSalt = passwordSalt
                };

                bool status = await _dapper.ExecuteWithParams(sql, param);

                if (status)
                {
                    return Ok(new { message = "Password reset successful." });
                }
                else
                {
                    return BadRequest(new { errorMessage = "Failed to reset password." });
                }
            }
        }
    }
}