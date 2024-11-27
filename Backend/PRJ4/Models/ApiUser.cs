using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PRJ4.Models
{
    public class ApiUser : IdentityUser
    {
        [MaxLength(100)]
        public string? FullName { get; set; }

        // Add more properties to get different user information
        public virtual ICollection<Vudgifter> Vudgifters { get; set; } = new List<Vudgifter>();
        public virtual ICollection<Fudgifter> Fudgifters { get; set; } = new List<Fudgifter>();

    }
}