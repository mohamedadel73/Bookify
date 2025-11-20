using Microsoft.AspNetCore.Mvc;
using Bookify.service.Repositories;
using Bookify.wep.Models.Bookings;
using Bookify.Domain.Entities;

namespace Bookify.wep.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IBookingRepository _bookings;

        public BookingsController(IBookingRepository bookings)
        {
            _bookings = bookings;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var items = await _bookings.ListAsync();
            return View(items.Select(b => b.ToDto()).ToList());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookings.GetByIdAsync(id);
            if (booking == null) return NotFound();

            return View(booking.ToDto());
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var entity = dto.ToEntity();
            await _bookings.AddAsync(entity);

            return RedirectToAction(nameof(Index));
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _bookings.GetByIdAsync(id);
            if (booking == null) return NotFound();

            var dto = new UpdateBookingDto(
                booking.RoomId,
                booking.Gid,
                booking.CheckIn,
                booking.CheckOut,
                booking.BookingStatus
            );

            return View(dto);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateBookingDto dto)
        {
            var booking = await _bookings.GetByIdAsync(id);
            if (booking == null) return NotFound();

            if (!ModelState.IsValid)
                return View(dto);

            dto.Apply(booking);
            await _bookings.UpdateAsync(booking);

            return RedirectToAction(nameof(Index));
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _bookings.GetByIdAsync(id);
            if (booking == null) return NotFound();

            return View(booking.ToDto());
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookings.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
