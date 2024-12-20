using System.ComponentModel.DataAnnotations;

namespace PRJ4.DTOs
{
    public class ResetPasswordDTO
    {
      [Required(ErrorMessage = "Password is required")]
      public string? Password { get; set; }

      [Compare("Password", ErrorMessage = "Passwords do not match")]
      public string ConfirmPassword { get; set; }
      
      public string? Token { get; set; }
      public string? Email { get; set; }
    }
}