using System.ComponentModel.DataAnnotations;

namespace PRJ4.DTOs
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Password is required")]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        
    }
}