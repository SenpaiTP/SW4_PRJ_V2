using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PRJ4.Models
{
    public class ApiUser : IdentityUser
    {
        [MaxLength(100)]
        public string? FullName { get; set; }

        // Add more properties to get different user information

    }
}