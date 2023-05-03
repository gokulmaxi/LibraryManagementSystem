using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Areas.Identity.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly LibraryManagementSystemContext _context;

        public ReservationController(LibraryManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
              return _context.ReservationDetails != null ? 
                          View(await _context.ReservationDetails.ToListAsync()) :
                          Problem("Entity set 'LibraryManagementSystemContext.ReservationDetails'  is null.");
        }

        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReservationDetails == null)
            {
                return NotFound();
            }

            var reservationDetails = await _context.ReservationDetails
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservationDetails == null)
            {
                return NotFound();
            }

            return View(reservationDetails);
        }

        // GET: Reservation/Create
        public IActionResult Create(int id)
        {
            Console.WriteLine(id);
            return View();
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationId,ReservedDate,IssuedDate,ReturnDate,ReturnedDate")] ReservationDetails reservationDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservationDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservationDetails);
        }

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReservationDetails == null)
            {
                return NotFound();
            }

            var reservationDetails = await _context.ReservationDetails.FindAsync(id);
            if (reservationDetails == null)
            {
                return NotFound();
            }
            return View(reservationDetails);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationId,ReservedDate,IssuedDate,ReturnDate,ReturnedDate")] ReservationDetails reservationDetails)
        {
            if (id != reservationDetails.ReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationDetailsExists(reservationDetails.ReservationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reservationDetails);
        }

        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReservationDetails == null)
            {
                return NotFound();
            }

            var reservationDetails = await _context.ReservationDetails
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservationDetails == null)
            {
                return NotFound();
            }

            return View(reservationDetails);
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReservationDetails == null)
            {
                return Problem("Entity set 'LibraryManagementSystemContext.ReservationDetails'  is null.");
            }
            var reservationDetails = await _context.ReservationDetails.FindAsync(id);
            if (reservationDetails != null)
            {
                _context.ReservationDetails.Remove(reservationDetails);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationDetailsExists(int id)
        {
          return (_context.ReservationDetails?.Any(e => e.ReservationId == id)).GetValueOrDefault();
        }
    }
}
