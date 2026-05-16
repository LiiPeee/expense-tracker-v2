using BudgetTracker.Core.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Resend;

namespace BudgetTracker.Infrastructure.Services;

public class EmailService(IResend resend, IConfiguration configuration) : IEmailService
{
    public async Task SendCodeToEmailAsync(string email, string id, string token)
    {
        var url = $"{configuration["FrontEndUrl"]}/verify-email/{id}";
        var htmlBody = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head><meta charset='UTF-8' /></head>
            <body style='font-family: Arial, sans-serif; background-color:#f4f4f4; padding: 40px;'>
                <table width='600' style='background:#fff; border-radius:8px; padding:32px; margin:auto;'>
                    <tr>
                        <td style='background:#4F46E5; padding:24px; text-align:center;'>
                            <h1 style='color:#fff; margin:0;'>Expense Tracker</h1>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding:32px;'>
                            <h2 style='color:#111827;'>Confirme sua conta</h2>
                            <p style='color:#6B7280; font-size:16px; line-height:1.6;'>
                                Obrigado por se cadastrar! Clique no botão abaixo para verificar seu email.
                            </p>
                            <div style='text-align:center; margin:32px 0;'>
                                <a href='{url}'
                                   style='background:#4F46E5; color:#fff; padding:14px 32px;
                                          border-radius:6px; font-size:16px; text-decoration:none;'>
                                    Verificar minha conta
                                </a>
                            </div>
                            <p style='color:#9CA3AF; font-size:13px;'>
                                Seu token: <strong>{token}</strong>
                            </p>
                            <p style='color:#9CA3AF; font-size:13px;'>
                                Se não criou uma conta, ignore este email.
                            </p>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";

        var message = new EmailMessage
        {
            From = configuration["EmailSender:From"]!,
            Subject = "Account Confirmation",
            HtmlBody = htmlBody,
        };
        message.To.Add(email);

        await resend.EmailSendAsync(message);
    }

    public async Task SendVerificationEmailAsync(string email, string token)
    {
        var url = $"{configuration["FrontEndUrl"]}/reset-password?email={email}&token={token}";
        var htmlBody = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head><meta charset='UTF-8' /></head>
            <body style='font-family: Arial, sans-serif; background-color:#f4f4f4; padding: 40px;'>
                <table width='600' style='background:#fff; border-radius:8px; padding:32px; margin:auto;'>
                    <tr>
                        <td style='background:#4F46E5; padding:24px; text-align:center;'>
                            <h1 style='color:#fff; margin:0;'>Expense Tracker</h1>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding:32px;'>
                            <h2 style='color:#111827;'>Redefinir senha</h2>
                            <p style='color:#6B7280; font-size:16px; line-height:1.6;'>
                                Clique no botão abaixo para redefinir sua senha.
                            </p>
                            <div style='text-align:center; margin:32px 0;'>
                                <a href='{url}'
                                   style='background:#4F46E5; color:#fff; padding:14px 32px;
                                          border-radius:6px; font-size:16px; text-decoration:none;'>
                                    Verificar minha conta
                                </a>
                            </div>
                            <p style='color:#9CA3AF; font-size:13px;'>
                                Seu token: <strong>{token}</strong>
                            </p>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";

        var message = new EmailMessage
        {
            From = configuration["EmailSender:From"]!,
            Subject = "Reset password",
            HtmlBody = htmlBody,
        };
        message.To.Add(email);

        await resend.EmailSendAsync(message);
    }
}



