
using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace backend.Infrastructure.Database
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options)
    {
    }

    public DbSet<Loan> Loans { get; set; }
    public DbSet<LoanApplication> LoanApplications { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<RefreshTokens> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserActionLog> UserActionLogs { get; set; }
    public DbSet<SettingsData> SettingsData { get; set; }
    public DbSet<RejectionReason> RejectionReasons { get; set; }
    public DbSet<LoanApplicationStatusType> LoanApplicationStatusTypes { get; set; }
    public DbSet<LoanStatusType> LoanStatusTypes { get; set; }
    public DbSet<PaymentStatusType> PaymentStatusTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }
  }
}
