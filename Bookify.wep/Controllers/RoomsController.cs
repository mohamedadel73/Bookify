using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Bookify.service.Repositories;
using Bookify.Domain.Entities;
using Bookify.wep.Models.Rooms;

namespace Bookify.wep.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomRepository _rooms;

        public RoomsController(IRoomRepository rooms)
        {
            _rooms = rooms;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _rooms.ListAsync();
            return Ok(items.Select(r => r.ToDto()).ToList());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _rooms.GetByIdAsync(id);
            if (room == null) return NotFound();
            return Ok(room.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomDto dto)
        {
            var entity = dto.ToEntity();
            await _rooms.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.RoomId }, entity.ToDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomDto dto)
        {
            var existing = await _rooms.GetByIdAsync(id);
            if (existing == null) return NotFound();
            dto.Apply(existing);
            await _rooms.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _rooms.DeleteAsync(id);
            return NoContent();
        }
    }
}


