using System.Security.Cryptography;

namespace Services.services
{
    public static class PasswordHasher
    {
        private const int Iterations = 100000;
        private const int SaltSize = 16;
        private const int KeySize = 32;
        /// Створює хеш пароля та генерує випадкову сіль.
        /// </summary>
        /// <param name="password">Пароль для хешування.</param>
        /// <returns>Рядок, що містить сіль, хеш та параметри.</returns>
        public static string HashPassword(string password)
        {
            byte[] salt;
            using (var rng = RandomNumberGenerator.Create())
            {
                salt = new byte[SaltSize];
                rng.GetBytes(salt);
            }

            byte[] hash;
            var algorithm = HashAlgorithmName.SHA256;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, algorithm))
            {
                hash = pbkdf2.GetBytes(KeySize);
            }

            return $"{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        /// <summary>
        /// Перевіряє введений пароль порівняно із збереженим хешем.
        /// </summary>
        /// <param name="password">Пароль, введений користувачем.</param>
        /// <param name="hashedPassword">Збережений хеш (з сіллю).</param>
        /// <returns>True, якщо паролі збігаються, інакше False.</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split(':');
            if (parts.Length != 3)
            {
                return false;
            }

            var iterations = int.Parse(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var storedHash = Convert.FromBase64String(parts[2]);

            byte[] computedHash;
            var algorithm = HashAlgorithmName.SHA256;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, algorithm))
            {
                computedHash = pbkdf2.GetBytes(storedHash.Length);
            }

            return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
        }
    }
}
