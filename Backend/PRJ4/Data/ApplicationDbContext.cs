using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PRJ4.Models;

namespace PRJ4.Data;

public partial class ApplicationDbContext : IdentityDbContext<ApiUser>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Kategori> Kategorier {get; set;}
    public DbSet<Fudgifter> Fudgifters { get; set; }
    public DbSet<Vindtægt> Vindtægter { get; set; }
    public DbSet<Findtægt> Findtægter { get; set; }
    public DbSet<Vudgifter> Vudgifters { get; set; }
    public DbSet<ApiUser> ApiUsers { get; set; }
    public DbSet<LoginModel> LoginModels { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<CategoryLimit> CategoryLimits { get; set; }
     public DbSet<Saving> Savings { get; set; }
    public DbSet<Indstillinger> Indstillingers {get; set;}
}