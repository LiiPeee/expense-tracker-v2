using BudgetTracker.Core.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BudgetTracker.Infrastructure.Services;

public class EmailService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IEmailService
{
    private const string BrevoApiUrl = "https://api.brevo.com/v3/smtp/email";

    private async Task SendAsync(string toEmail, string subject, string htmlBody)
    {
        var payload = new
        {
            sender = new
            {
                name = configuration["EmailSender:SenderName"] ?? "Expense Tracker",
                email = configuration["EmailSender:From"]
            },
            to = new[] { new { email = toEmail } },
            subject,
            htmlContent = htmlBody
        };

        var client = httpClientFactory.CreateClient("Brevo");
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(BrevoApiUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Brevo error {(int)response.StatusCode}: {error}");
        }
    }

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
                                Se não criou uma conta, ignore este email.
                            </p>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";

        await SendAsync(email, "Account Confirmation", htmlBody);
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
                                    Redefinir minha senha
                                </a>
                            </div>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";

        await SendAsync(email, "Reset password", htmlBody);
    }
}

