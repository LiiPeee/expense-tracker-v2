namespace ExpenseTrackerV2.Core.Domain.Options;

public sealed class EncryptionOptions
{
    public string Base64EncryptionKey { get; init; } = string.Empty;
    public string Base64EncryptionIV { get; init; } = string.Empty;
}