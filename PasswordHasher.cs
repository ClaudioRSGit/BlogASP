using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BlogASP

{
    public class PasswordHasher
    {

        public static string HashPassword(string password)
        {
            byte[] salt = GenerateSalt();

            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000, 
                numBytesRequested: 256 / 8 
            ));

            return $"{Convert.ToBase64String(salt)}.{hash}";
        }

        public static bool VerifyPassword(string hashedPassword, string userPassword)
        {
            string[] parts = hashedPassword.Split('.');
            if (parts.Length != 2)
                return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            string hash = parts[1];

            string computedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: userPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000, 
                numBytesRequested: 256 / 8 
            ));

            return hash == computedHash;
        }

        private static byte[] GenerateSalt()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[128 / 8];
                rng.GetBytes(salt);
                return salt;
            }
        }
    }
}
