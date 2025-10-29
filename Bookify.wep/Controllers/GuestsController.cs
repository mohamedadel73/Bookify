using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Bookify.service.Repositories;
using Bookify.Domain.Entities;
using Bookify.wep.Models.Guests;

namespace Bookify.wep.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestsController : ControllerBase
    {
        private readonly IGuestRepository _guests;

        public GuestsController(IGuestRepository guests)
        {
            _guests = guests;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _guests.ListAsync();
            return Ok(items.Select(g => g.ToDto()).ToList());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var guest = await _guests.GetByIdAsync(id);
            if (guest == null) return NotFound();
            return Ok(guest.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGuestDto dto)
        {
            var entity = dto.ToEntity();
            await _guests.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Gid }, entity.ToDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateGuestDto dto)
        {
            var existing = await _guests.GetByIdAsync(id);
            if (existing == null) return NotFound();
            dto.Apply(existing);
            await _guests.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _guests.DeleteAsync(id);
            return NoContent();
        }
    }
}


