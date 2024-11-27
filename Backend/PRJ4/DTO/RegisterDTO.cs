using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PRJ4.Models;
using System.ComponentModel.DataAnnotations;

namespace PRJ4.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string? Fornavn{ get; set; }
        [Required]
        public string? Efternavn{ get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}