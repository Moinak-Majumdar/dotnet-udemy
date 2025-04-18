using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace user_auth.helper
{
    public class PasswordHelper(IConfiguration config)
    {
        public byte[] GetPasswordHash(byte[] salt, string password)
        {
            string paper = config.GetSection("AppSettings:PasswordKey").Value ?? "";
            string passwordWithPaper = password + paper;

            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: passwordWithPaper,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32
            );

            return passwordHash;
        }
    }
}