using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ExpenseTrackerV2.Core.Domain.Utils
{
    public static class PasswordHelper
    {
        private static readonly IConfiguration _config;
        private static readonly string Base64EncryptionKey = _config["Base64EncryptionKey"];
        private static readonly string Base64EncryptionIV = _config["Base64EncryptionIV"];

        public static string Encrypt(string plainText) 
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            using Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(Base64EncryptionKey);
            aes.IV = Convert.FromBase64String(Base64EncryptionIV);

            using MemoryStream ms = new MemoryStream();

            using(CryptoStream cs = new CryptoStream(ms,aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(plainBytes, 0, plainBytes.Length);
                cs.FlushFinalBlock();
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] encryptdBytes = Convert.FromBase64String(encryptedText);

            using Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(Base64EncryptionKey);
            aes.IV = Convert.FromBase64String(Base64EncryptionIV);

            using MemoryStream ms  = new MemoryStream();

            using(CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(encryptdBytes, 0, encryptdBytes.Length);
                cs.FlushFinalBlock();
            }

            return Encoding.UTF8.GetString(ms.ToArray());
        }
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RNGCryptoServiceProvider.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

    }
}
