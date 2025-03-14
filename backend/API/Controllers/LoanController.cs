using backend.Core.DTOs;
using backend.Core.Models;
using backend.Infrastructure.Repositories;
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetLoan(Guid id)
        {
            var loan = await _repository.GetLoanAsync(id);
            return Ok(loan);
        }

        [HttpGet]
        public async Task<IActionResult> GetLoans([FromQuery] PaginationParameters parameters)
        {
            var result = await _repository.GetLoansAsync(parameters);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] LoanDTO loanDto)
        {
            var loan = await _repository.CreateLoanAsync(loanDto);
            return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loan);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateLoan(Guid id, [FromBody] LoanDTO loanDto)
        {
            var updatedLoan = await _repository.UpdateLoanAsync(id, loanDto);
            return Ok(updatedLoan);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteLoan(Guid id)
        {
            await _repository.DeleteLoanAsync(id);
            return NoContent();
        }
    }
}
