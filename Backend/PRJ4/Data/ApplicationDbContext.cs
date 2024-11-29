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

    // public DbSet<Bruger> Brugers { get; set; }
    public DbSet<Kategori> Kategorier {get; set;}
    // public DbSet<Findtægt> Findtægts { get; set; }
    public DbSet<Fudgifter> Fudgifters { get; set; }
    // public DbSet<Vindtægter> Vindtægters { get; set; }
    public DbSet<Vudgifter> Vudgifters { get; set; }
    //public DbSet<Bruger> Brugers { get; set; }
    public DbSet<ApiUser> ApiUsers { get; set; }
    public DbSet<LoginModel> LoginModels { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    // public DbSet<LoginModel> LoginModels { get; set; }

  
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Bruger>().ToTable("Bruger");
    //     modelBuilder.Entity<Budget>().ToTable("Budget");
    //     modelBuilder.Entity<Findtægt>().ToTable("Findtægt");
    //     modelBuilder.Entity<Kategori>().ToTable("Kategori");
    //     modelBuilder.Entity<Vindtægter>().ToTable("Vintægter");
    //     modelBuilder.Entity<Vudgifter>().ToTable("Vudgifter");
    //     modelBuilder.Entity<Kategori>().ToTable("Kategorier");
    //     modelBuilder.Entity<LoginModel>().ToTable("LoginModel");
    
    //     base.OnModelCreating(modelBuilder);
    // }
}