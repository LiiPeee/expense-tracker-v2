using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerV2.WebApi.Models.Auth
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Senha deve ter entre 8 e 20 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "Senha deve conter: letra minúscula, maiúscula, número e caractere especial")]
        public string NewPassword { get; set; }
    }
}
