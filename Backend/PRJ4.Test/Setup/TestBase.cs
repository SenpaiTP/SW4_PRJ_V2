using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRJ4.Data;
using PRJ4.Models;

namespace PRJ4.Test.Setup
{
        public abstract class TestBase
    {
        protected DbContextOptions<ApplicationDbContext> DbContextOptions;

        public TestBase()
        {
            // Create unique database names for isolation
            DbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        protected ApplicationDbContext CreateContext()
        {
            var context = new ApplicationDbContext(DbContextOptions);
            SeedIdentityData(context);
            return context;
        }
        private void SeedIdentityData(ApplicationDbContext context)
        {
            var passwordHasher = new PasswordHasher<ApiUser>();

            // Seed a test user
            var testUser = new ApiUser
            {
                Id = "user-1",
                UserName = "testuser@example.com",
                Email = "testuser@example.com",
                FullName = "TestUser",
                NormalizedUserName = "TESTUSER",
                NormalizedEmail = "TESTUSER@EXAMPLE.COM",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            testUser.PasswordHash = passwordHasher.HashPassword(testUser, "Test123!");

            // Add the user to the database
            context.Users.Add(testUser);
            context.SaveChanges();
        }
        
    }

}