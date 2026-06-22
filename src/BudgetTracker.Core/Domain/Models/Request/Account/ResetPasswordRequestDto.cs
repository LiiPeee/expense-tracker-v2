
namespace BudgetTracker.Core.Domain.Models.Request.Account
{
    public class ResetPasswordRequestDto
    {
        public string email { get; set; }
        public string newPassword { get; set; }
        public string token { get; set; }
    }
}


