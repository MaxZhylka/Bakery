using System.ComponentModel.DataAnnotations;
using backend.Core.Attributes;
using backend.Core.Enums;

namespace backend.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [Password]
        [Required]
        public required string Password { get; set; }

        [Required]
        public required UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Loan> Loans { get; set; } = new();
        public List<LoanApplication> LoanApplications { get; set; } = new();
        public List<Payment> Payments { get; set; } = new();
        public List<RefreshTokens> RefreshTokens { get; set; } = new();
        public List<UserActionLog> ActionLogs { get; set; } = new();
    }
}
