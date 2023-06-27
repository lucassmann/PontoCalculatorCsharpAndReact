using System.ComponentModel.DataAnnotations;

namespace PontoCalculator.Dtos
{
    public class ResetPasswordDto
    {
        [Required]
        public string PasswordResetToken { get; set; } = string.Empty;
        [Required] public string newPassword { get; set; } = string.Empty;
    }
}
