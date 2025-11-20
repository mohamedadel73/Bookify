using Microsoft.AspNetCore.Mvc;
using Bookify.service.Repositories;
using Bookify.wep.Models.Rooms;

namespace Bookify.wep.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomRepository _rooms;

        public RoomsController(IRoomRepository rooms)
        {
            _rooms = rooms;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _rooms.ListAsync();
            return View(items.Select(r => r.ToDto()).ToList());
        }

        public async Task<IActionResult> Details(int id)
        {
            var room = await _rooms.GetByIdAsync(id);
            if (room == null) return NotFound();

            return View(room.ToDto());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _rooms.AddAsync(dto.ToEntity());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var room = await _rooms.GetByIdAsync(id);
            if (room == null) return NotFound();

            var dto = new UpdateRoomDto(room.RoomType, room.Rstatus, room.Price);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateRoomDto dto)
        {
            var room = await _rooms.GetByIdAsync(id);
            if (room == null) return NotFound();

            if (!ModelState.IsValid)
                return View(dto);

            dto.Apply(room);
            await _rooms.UpdateAsync(room);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var room = await _rooms.GetByIdAsync(id);
            if (room == null) return NotFound();

            return View(room.ToDto());
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _rooms.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
