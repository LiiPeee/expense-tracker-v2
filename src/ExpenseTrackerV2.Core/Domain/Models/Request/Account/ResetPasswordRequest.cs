
namespace ExpenseTrackerV2.Core.Domain.Models.Request.Account
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
