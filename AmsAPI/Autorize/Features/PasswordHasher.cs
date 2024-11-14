using AmsAPI.Autorize.Interfaces;
using System.Security.Cryptography;

namespace AmsAPI.Autorize.Features
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SALT_SIZE = 16;
        private const int HASH_SIZE = 32;
        private const int ITERATIONS = 10000;
        private readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;
        public (string, string) Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SALT_SIZE);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, ITERATIONS, _algorithm, HASH_SIZE);

            return (Convert.ToHexString(hash), Convert.ToHexString(salt));
        }

        public bool Verify(string password, string passwordHash, string salt)
        {
            byte[] saltBytes = Convert.FromHexString(salt);
            byte[] hashBytes = Convert.FromHexString(passwordHash);
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, ITERATIONS, _algorithm, HASH_SIZE);
            return CryptographicOperations.FixedTimeEquals(hashBytes, inputHash);
        }
    }
}
