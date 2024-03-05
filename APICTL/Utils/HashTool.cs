using System.Security.Cryptography;

namespace APICTL.Utils
{
    public class HashTool
    {
        public static byte[] GenerateSalt()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[16]; // 16 bytes for the salt
                rng.GetBytes(salt);
                return salt;
            }
        }

        public static string HashPassword(string password, byte[] salt)
        {
            int iterations = 10000; // Number of iterations for PBKDF2

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); // 32 bytes for the hashed password
                return Convert.ToBase64String(hash);
            }
        }
    }
}
