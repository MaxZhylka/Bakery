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

	public static async Task SeedDefaultSettings(AppDbContext context)
	{
		if (await context.SettingsData.AnyAsync())
		{
			return;
		}

		context.SettingsData.Add(new SettingsData
		{
			Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
			StartReportDate = new DateTime(2024, 01, 01),
			EndReportDate = new DateTime(2026, 12, 31),
			BackupPath = @"C:\Backups"
		});

		await context.SaveChangesAsync();
	}


	public static async Task SeedStatusTypesAsync(AppDbContext context)
	{
		if (await context.LoanStatusTypes.AnyAsync()
				|| await context.LoanApplicationStatusTypes.AnyAsync()
				|| await context.PaymentStatusTypes.AnyAsync())
			return;

		var loanStatuses = new List<LoanStatusType>
		{
				new() { Name = "Active"       },
				new() { Name = "GetMoney"     },
				new() { Name = "NeedPayment"  },
				new() { Name = "Completed"    },
		};

		var loanApplicationStatuses = new List<LoanApplicationStatusType>
		{
				new() { Name = "Moderation" },
				new() { Name = "Approved"   },
				new() { Name = "Rejected"   },
		};

		var paymentStatuses = new List<PaymentStatusType>
		{
				new() { Name = "Active"    },
				new() { Name = "Completed" },
		};

		context.LoanStatusTypes.AddRange(loanStatuses);
		context.LoanApplicationStatusTypes.AddRange(loanApplicationStatuses);
		context.PaymentStatusTypes.AddRange(paymentStatuses);

		await context.SaveChangesAsync();
	}

}
