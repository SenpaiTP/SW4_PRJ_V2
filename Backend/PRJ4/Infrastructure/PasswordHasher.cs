using System.Security.Cryptography;

public class PasswordHasher
{
    public static string HashPassword(string password)
    {
        using(var hmac=new HMACSHA256())
        {
           var salt=hmac.Key;
           var hash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
           return Convert.ToBase64String(salt)+Convert.ToBase64String(hash);
        }
    }
}