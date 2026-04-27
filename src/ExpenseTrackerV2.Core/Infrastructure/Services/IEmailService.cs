using System;

namespace ExpenseTrackerV2.Core.Infrastructure.Services;

public interface IEmailService
{
    Task SendCodeToEmailAsync(string email,string id, string token);
    Task SendVerificationEmailAsync(string email, string token);
}
