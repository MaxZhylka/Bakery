using backend.Core.DTOs;
using backend.Core.Models;
using Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace backend.Api.Controllers
{
    [ApiController]
    [Route("api/loans")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanRepository _repository;

        public LoanController(ILoanRepository repository)
        {
            _repository = repository;
        }

        [ErrorHandler]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetLoan(Guid id)
        {
            var loan = await _repository.GetLoanAsync(id);
            return Ok(loan);
        }

        [ErrorHandler]
        [HttpGet]
        public async Task<IActionResult> GetLoans([FromQuery] PaginationParameters parameters)
        {
            var result = await _repository.GetLoansAsync(parameters);
            return Ok(result);
        }

        [ErrorHandler]
        [HttpGet]
        [Route("user/{userId:guid}")]
        public async Task<IActionResult> GetLoansByUserId(Guid userId, [FromQuery] PaginationParameters parameters)
        {
            var result = await _repository.GetLoansByUserIdAsync(userId, parameters);
            return Ok(result);
        }

        [ErrorHandler]
        [HttpPost("{id:guid}")]

        public async Task<IActionResult> GetMoneyByLoanId(Guid id)
        {
            await _repository.GetMoneyByLoanIdAsync(id);
            return Ok();
        }

        [ErrorHandler]
        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] LoanDTO loanDto)
        {
            var loan = await _repository.CreateLoanAsync(loanDto);
            return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loan);
        }

        [ErrorHandler]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateLoan(Guid id, [FromBody] LoanDTO loanDto)
        {
            var updatedLoan = await _repository.UpdateLoanAsync(id, loanDto);
            return Ok(updatedLoan);
        }

        [ErrorHandler]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteLoan(Guid id)
        {
            await _repository.DeleteLoanAsync(id);
            return NoContent();
        }
    }
}
