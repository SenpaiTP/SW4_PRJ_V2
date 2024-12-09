using PRJ4.Models;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework.Legacy;
using PRJ4.Test.Setup;

[TestFixture]
public class ApplicationDbContextIdentityTests : TestBase
{
    [Test]
    public void CanRetrieveApiUser()
    {
        // Arrange
        using var context = CreateContext();
        
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

        var passwordHasher = new PasswordHasher<ApiUser>();

        var user = context.Users.FirstOrDefault(u => u.UserName == "testuser@example.com");
        ClassicAssert.IsNotNull(user);

        // Act
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, "Test123!");

        // Assert
        ClassicAssert.AreEqual(PasswordVerificationResult.Success, result);
    }
}
