
namespace ExpenseTrackerV2.Core.Domain.Models.Request.Account
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
