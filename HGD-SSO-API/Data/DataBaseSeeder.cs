using HGD_SSO_API.Data;
using HGD_SSO_API.Models.UserManagement;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

public static class DatabaseSeeder
{
    public static void SeedUsers(AppDbContext context)
    {
        context.Database.Migrate();

        if (!context.Users.Any())
        {
            var plainPassword = "1234"; 
            var testUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                PasswordHash = HashPassword(plainPassword),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Users.Add(testUser);
            context.SaveChanges();
        }
    }

    private static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}