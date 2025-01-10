using System.ComponentModel.DataAnnotations;

namespace UserService.DTOs
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = string.Empty;
    }
}
