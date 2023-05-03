using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Areas.Identity.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly LibraryManagementSystemContext _context;
        private readonly UserManager<LMSUser> _userManager;

        public ReservationController(LibraryManagementSystemContext context, UserManager<LMSUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            return _context.ReservationDetails != null ?
                        View(await _context.ReservationDetails
                        .Include(d => d.Book)
                        .Include(d => d.ReservedUser)
                        .ToListAsync()) :
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
            TempData["BookId"] = id;
            return View();
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationDetails reservationDetails)
        {
            try
            {
                int tempId = (int)TempData["BookId"];
                var BookData = await _context.BookDetails.Where(x => x.BookId == tempId).FirstOrDefaultAsync();
                reservationDetails.ReservedUser = await _userManager.GetUserAsync(User);
                reservationDetails.Book = BookData;
                Console.WriteLine("\n \n" + reservationDetails.ReservedDate.ToString() + " return" + reservationDetails.ReturnDate.ToString());
                _context.Add(reservationDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(reservationDetails);
            }
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
        public async Task<IActionResult> Edit(int id, [Bind("ReservationId,ReservedDate,IssuedDate,ReturnDate,ReturnedDate,ReservationStatus")] ReservationDetails reservationDetails)
        {
            if (id != reservationDetails.ReservationId)
            {
                return NotFound();
            }
            try
            {
                // change the reservation status
                if (reservationDetails.ReservationStatus == ReservationStatus.Reserved)
                    reservationDetails.ReservationStatus = ReservationStatus.Issued;
                else if (reservationDetails.ReservationStatus == ReservationStatus.Issued)
                {
                    reservationDetails.ReturnedDate = DateTime.Now;
                    reservationDetails.ReservationStatus = ReservationStatus.Returned;
                    // check for fine
                    if (reservationDetails.ReturnDate < reservationDetails.ReturnedDate)
                    {
                        TimeSpan dateDiff = (TimeSpan)(reservationDetails.ReturnedDate - reservationDetails.ReturnDate);
                        Console.Write("\n \n \n");
                        Console.WriteLine(dateDiff.Days);
                    }
                }
                Console.WriteLine("updating");
                _context.Update(reservationDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
            return View();
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
