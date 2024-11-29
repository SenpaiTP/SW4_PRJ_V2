using NUnit.Framework;
using PRJ4.Data;
using PRJ4.Models;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework.Legacy;

[TestFixture]
public class ApplicationDbContextIdentityTests : TestBase
{
    [Test]
    public void CanRetrieveApiUser()
    {
        // Arrange
        using var context = CreateContext();
        context.Database.EnsureDeleted();  // Delete any existing data
        context.Database.EnsureCreated();
        

        // Act
        var user = context.Users.FirstOrDefault(u => u.UserName == "testuser@example.com");
        
        // Assert
        ClassicAssert.IsNotNull(user);
        ClassicAssert.AreEqual("testuser@example.com", user.Email);
        ClassicAssert.AreEqual("TestUser", user.FullName);
        ClassicAssert.AreEqual("testuser@example.com", user.UserName);
    }

    [Test]
    public void PasswordHasherValidatesPassword()
    {
        // Arrange
        using var context = CreateContext();
        context.Database.EnsureDeleted();  // Delete any existing data
        context.Database.EnsureCreated();
        var passwordHasher = new PasswordHasher<ApiUser>();

        var user = context.Users.FirstOrDefault(u => u.UserName == "testuser@example.com");
        ClassicAssert.IsNotNull(user);

        // Act
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, "Test123!");

        // Assert
        ClassicAssert.AreEqual(PasswordVerificationResult.Success, result);
    }
}
