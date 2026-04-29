using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerV2.WebApi.Models.Auth
{
    public class VerifyTokenRequest
    {
        public string id { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
