using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.WebApi.Models.Auth
{
    public class VerifyTokenRequest
    {
        public string Id { get; set; }

        [Required]
        public string Token { get; set; }
    }
}


