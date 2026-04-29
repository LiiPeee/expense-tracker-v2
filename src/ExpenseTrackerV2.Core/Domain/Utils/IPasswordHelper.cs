namespace ExpenseTrackerV2.Core.Domain.Utils;

public interface IPasswordHelper
{
    string Encrypt(string plainText);
    string Decrypt(string encryptedText);
    string GenerateRefreshToken();
    string GenerateVerificationCode();
    string EncryptUrl(string plainText);
    string DecryptUrl(string encryptedText);
}