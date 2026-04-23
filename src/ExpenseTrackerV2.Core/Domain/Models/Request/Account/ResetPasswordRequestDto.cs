
namespace ExpenseTrackerV2.Core.Domain.Models.Request.Account
{
    public class ResetPasswordRequestDto
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
