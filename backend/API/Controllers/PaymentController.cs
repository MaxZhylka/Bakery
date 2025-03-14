using backend.Core.DTOs;
using backend.Core.Models;
using backend.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Api.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _repository;

        public PaymentController(IPaymentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPayment(Guid id)
        {
            var payment = await _repository.GetPaymentAsync(id);
            return Ok(payment);
        }

        [HttpGet]
        public async Task<IActionResult> GetPayments([FromQuery] PaginationParameters parameters)
        {
            var result = await _repository.GetPaymentsAsync(parameters);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDTO paymentDto)
        {
            var payment = await _repository.CreatePaymentAsync(paymentDto);
            return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePayment(Guid id, [FromBody] PaymentDTO paymentDto)
        {
            var updatedPayment = await _repository.UpdatePaymentAsync(id, paymentDto);
            return Ok(updatedPayment);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePayment(Guid id)
        {
            await _repository.DeletePaymentAsync(id);
            return NoContent();
        }
    }
}
