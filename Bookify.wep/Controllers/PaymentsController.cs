using Microsoft.AspNetCore.Mvc;
using Bookify.service.Repositories;
using Bookify.wep.Models.Payments;

namespace Bookify.wep.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IPaymentRepository _payments;

        public PaymentsController(IPaymentRepository payments)
        {
            _payments = payments;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _payments.ListAsync();
            return View(items.Select(p => p.ToDto()).ToList());
        }

        public async Task<IActionResult> Details(int id)
        {
            var payment = await _payments.GetByIdAsync(id);
            if (payment == null) return NotFound();

            return View(payment.ToDto());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _payments.AddAsync(dto.ToEntity());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var payment = await _payments.GetByIdAsync(id);
            if (payment == null) return NotFound();

            var dto = new UpdatePaymentDto(
                payment.BookingId,
                payment.StripePaymentIntentId,
                payment.Status,
                payment.Amount,
                payment.PaymentDate
            );

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdatePaymentDto dto)
        {
            var payment = await _payments.GetByIdAsync(id);
            if (payment == null) return NotFound();

            if (!ModelState.IsValid)
                return View(dto);

            dto.Apply(payment);
            await _payments.UpdateAsync(payment);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _payments.GetByIdAsync(id);
            if (payment == null) return NotFound();

            return View(payment.ToDto());
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _payments.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
