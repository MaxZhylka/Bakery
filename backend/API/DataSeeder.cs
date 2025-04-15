using System.Security.Cryptography;
using System.Text;
using backend.Core.Entities;
using backend.Core.Enums;
using backend.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DataSeeder
{
    public static async Task SeedAdminUserAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
        {
            return;
        }

        PasswordHasher<string> _passwordHasher = new();

        var adminUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Email = "admin@gmail.com",
            Password = _passwordHasher.HashPassword("admin@gmail.com", "Ak785532#"),
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(adminUser);

        await context.SaveChangesAsync();
    }

}
