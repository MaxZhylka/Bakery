using backend.Core.DTOs;
using backend.Core.Entities;
using backend.Core.Enums;
using backend.Core.Models;
using backend.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Core.Exceptions;

namespace backend.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly AppDbContext _context;

        public LoanRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoanDTO> GetLoanAsync(Guid id)
        {
            var loan = await _context.Loans
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (loan == null)
                throw new DatabaseOperationException(Operations.GetLoan, new Exception("Loan not found"));

            return new LoanDTO
            {
                Id = loan.Id,
                UserId = loan.UserId,
                Percent = loan.Percent,
                ValueToPayOnCurrentMonth = loan.ValueToPayOnCurrentMonth,
                ValueToPay = loan.ValueToPay,
                Status = loan.Status,
                CreatedAt = loan.CreatedAt
            };
        }

        public async Task<PaginatedResult<LoanDTO>> GetLoansAsync(PaginationParameters parameters)
        {
            var query = _context.Loans.Include(l => l.User).OrderByDescending(l => l.CreatedAt);

            var totalRecords = await query.CountAsync();

            var loans = await query
                .Skip(parameters.Offset * parameters.Size)
                .Take(parameters.Size)
                .Select(loan => new LoanDTO
                {
                    Id = loan.Id,
                    UserId = loan.UserId,
                    Percent = loan.Percent,
                    ValueToPayOnCurrentMonth = loan.ValueToPayOnCurrentMonth,
                    ValueToPay = loan.ValueToPay,
                    Status = loan.Status,
                    CreatedAt = loan.CreatedAt
                })
                .ToListAsync();

            return new PaginatedResult<LoanDTO>
            {
                Data = loans,
                Total = totalRecords,
            };
        }

        public async Task<LoanDTO> CreateLoanAsync(LoanDTO loanDto)
        {
            var loan = new Loan
            {
                Id = Guid.NewGuid(),
                UserId = loanDto.UserId,
                Percent = loanDto.Percent,
                ValueToPayOnCurrentMonth = loanDto.ValueToPayOnCurrentMonth,
                ValueToPay = loanDto.ValueToPay,
                Status = loanDto.Status,
                CreatedAt = DateTime.UtcNow
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return new LoanDTO
            {
                Id = loan.Id,
                UserId = loan.UserId,
                Percent = loan.Percent,
                ValueToPayOnCurrentMonth = loan.ValueToPayOnCurrentMonth,
                ValueToPay = loan.ValueToPay,
                Status = loan.Status,
                CreatedAt = loan.CreatedAt
            };
        }

        public async Task<LoanDTO> UpdateLoanAsync(Guid id, LoanDTO updatedLoan)
        {
            var loan = await _context.Loans.FirstOrDefaultAsync(l => l.Id == id);

            if (loan == null)
                throw new DatabaseOperationException(Operations.UpdateLoan, new Exception("Loan not found"));

            loan.Percent = updatedLoan.Percent;
            loan.ValueToPayOnCurrentMonth = updatedLoan.ValueToPayOnCurrentMonth;
            loan.ValueToPay = updatedLoan.ValueToPay;
            loan.Status = updatedLoan.Status;

            await _context.SaveChangesAsync();

            return new LoanDTO
            {
                Id = loan.Id,
                UserId = loan.UserId,
                Percent = loan.Percent,
                ValueToPayOnCurrentMonth = loan.ValueToPayOnCurrentMonth,
                ValueToPay = loan.ValueToPay,
                Status = loan.Status,
                CreatedAt = loan.CreatedAt
            };
        }

        public async Task DeleteLoanAsync(Guid id)
        {
            var loan = await _context.Loans.FindAsync(id);

            if (loan == null)
                throw new DatabaseOperationException(Operations.DeleteLoan, new Exception("Loan not found"));

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
        }
    }
}
