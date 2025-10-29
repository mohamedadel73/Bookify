using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Bookify.service.Repositories;
using Bookify.Domain.Entities;
using Bookify.wep.Models.Payments;

namespace Bookify.wep.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _payments;

        public PaymentsController(IPaymentRepository payments)
        {
            _payments = payments;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _payments.ListAsync();
            return Ok(items.Select(p => p.ToDto()).ToList());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _payments.GetByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto)
        {
            var entity = dto.ToEntity();
            await _payments.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentDto dto)
        {
            var existing = await _payments.GetByIdAsync(id);
            if (existing == null) return NotFound();
            dto.Apply(existing);
            await _payments.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _payments.DeleteAsync(id);
            return NoContent();
        }
    }
}


