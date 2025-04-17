using backend.Core.DTOs;
using backend.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Api.Controllers
{
    [ApiController]
    [Route("api/loan-applications")]
    public class LoanApplicationController : ControllerBase
    {
        private readonly ILoanApplicationRepository _repository;

        public LoanApplicationController(ILoanApplicationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetLoanApplication(Guid id)
        {
            var application = await _repository.GetLoanApplicationAsync(id);
            return Ok(application);
        }

        [HttpGet]
        public async Task<IActionResult> GetLoanApplications([FromQuery] PaginationParameters parameters)
        {
            var result = await _repository.GetLoanApplicationsAsync(parameters);
            return Ok(result);
        }

        [HttpGet]
        [Route("user/{userId:guid}")]
        public async Task<IActionResult> GetLoanApplicationsByUserId(Guid userId, [FromQuery] PaginationParameters parameters)
        {
            var result = await _repository.GetLoanApplicationsByUserIdAsync(userId, parameters);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoanApplication([FromBody] CreateLoanApplicationDTO applicationDto)
        {
            var application = await _repository.CreateLoanApplicationAsync(applicationDto);
            return CreatedAtAction(nameof(GetLoanApplication), new { id = application.Id }, application);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateLoanApplication(Guid id, [FromBody] LoanApplicationDTO applicationDto)
        {
            var updatedApplication = await _repository.UpdateLoanApplicationAsync(id, applicationDto);
            return Ok(updatedApplication);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteLoanApplication(Guid id)
        {
            await _repository.DeleteLoanApplicationAsync(id);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [Route("approve/{id:guid}")]
        public async Task<IActionResult> ApproveLoanApplication(Guid id)
        {
            await _repository.ApproveLoanApplicationAsync(id);
            return Ok(new { message = "Loan application approved successfully." });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [Route("reject/{id:guid}")]
        public async Task<IActionResult> RejectLoanApplication(Guid id)
        {
            string reason = "";
            await _repository.RejectLoanApplicationAsync(id, reason);
            return Ok(new { message = "Loan application rejected successfully." });
        }
    }
}
