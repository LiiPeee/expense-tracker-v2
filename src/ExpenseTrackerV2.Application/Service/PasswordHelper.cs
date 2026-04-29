using ExpenseTrackerV2.Core.Domain.Options;
using ExpenseTrackerV2.Core.Domain.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseTrackerV2.Application.Service
{
    public sealed class PasswordHelper(IOptions<EncryptionOptions> options) : IPasswordHelper
    {
        private readonly string _base64EncryptionKey = options.Value.Base64EncryptionKey;
        private readonly string _base64EncryptionIV = options.Value.Base64EncryptionIV;


        public string Encrypt(string plainText) 
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            using Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(_base64EncryptionKey);
            aes.IV = Convert.FromBase64String(_base64EncryptionIV);

            using MemoryStream ms = new MemoryStream();

            using(CryptoStream cs = new CryptoStream(ms,aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(plainBytes, 0, plainBytes.Length);
                cs.FlushFinalBlock();
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public string EncryptUrl(string plainText)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            using Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(_base64EncryptionKey);
            aes.GenerateIV();

            using MemoryStream ms = new MemoryStream();

            ms.Write(aes.IV, 0, aes.IV.Length);

            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(plainBytes, 0, plainBytes.Length);
                cs.FlushFinalBlock();
            }
            return Convert.ToBase64String(ms.ToArray()).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        public string Decrypt(string encryptedText)
        {
            byte[] encryptdBytes = Convert.FromBase64String(encryptedText);

            using Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(_base64EncryptionKey);
            aes.IV = Convert.FromBase64String(_base64EncryptionIV);

            using MemoryStream ms  = new MemoryStream();

            using(CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(encryptdBytes, 0, encryptdBytes.Length);
                cs.FlushFinalBlock();
            }

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public string DecryptUrl(string encryptedText)
        {
            string base64 = encryptedText.Replace('-', '+').Replace('_', '/') + new string('=', (4 - encryptedText.Length % 4) % 4);
            byte[] encryptdBytes = Convert.FromBase64String(base64);

            using Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(_base64EncryptionKey);
            byte[] iv = encryptdBytes[..16];
            byte[] cipherText = encryptdBytes[16..];
            aes.IV = iv;
            using MemoryStream ms = new MemoryStream();

            using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cipherText, 0, cipherText.Length);
                cs.FlushFinalBlock();
            }

            return Encoding.UTF8.GetString(ms.ToArray());
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RNGCryptoServiceProvider.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
