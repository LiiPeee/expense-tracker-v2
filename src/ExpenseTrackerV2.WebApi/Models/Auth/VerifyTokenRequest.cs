using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerV2.WebApi.Models.Auth
{
    public class VerifyTokenRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
