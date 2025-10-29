using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Bookify.service.Repositories;
using Bookify.Domain.Entities;
using Bookify.wep.Models.Bookings;

namespace Bookify.wep.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingRepository _bookings;

        public BookingsController(IBookingRepository bookings)
        {
            _bookings = bookings;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _bookings.ListAsync();
            return Ok(items.Select(b => b.ToDto()).ToList());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookings.GetByIdAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            var entity = dto.ToEntity();
            await _bookings.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.BookingId }, entity.ToDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookingDto dto)
        {
            var existing = await _bookings.GetByIdAsync(id);
            if (existing == null) return NotFound();
            dto.Apply(existing);
            await _bookings.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookings.DeleteAsync(id);
            return NoContent();
        }
    }
}


