using System.ComponentModel.DataAnnotations;

namespace UserService.DTOs
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Otp is required.")]
        public string Otp { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string NewPassword { get; set; }
    }
}
