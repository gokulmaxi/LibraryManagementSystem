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
    public class FineController : Controller
    {
        private readonly LibraryManagementSystemContext _context;

        public FineController(LibraryManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Fine
        public async Task<IActionResult> Index()
        {
              return _context.FineDetails != null ? 
                          View(await _context.FineDetails.ToListAsync()) :
                          Problem("Entity set 'LibraryManagementSystemContext.FineDetails'  is null.");
        }

        // GET: Fine/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FineDetails == null)
            {
                return NotFound();
            }

            var fineDetails = await _context.FineDetails
                .FirstOrDefaultAsync(m => m.FineId == id);
            if (fineDetails == null)
            {
                return NotFound();
            }

            return View(fineDetails);
        }

        // GET: Fine/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fine/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FineId,FineAmount,Paid")] FineDetails fineDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fineDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fineDetails);
        }

        // GET: Fine/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FineDetails == null)
            {
                return NotFound();
            }

            var fineDetails = await _context.FineDetails.FindAsync(id);
            if (fineDetails == null)
            {
                return NotFound();
            }
            return View(fineDetails);
        }

        // POST: Fine/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FineId,FineAmount,Paid")] FineDetails fineDetails)
        {
            if (id != fineDetails.FineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fineDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FineDetailsExists(fineDetails.FineId))
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
            return View(fineDetails);
        }

        // GET: Fine/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FineDetails == null)
            {
                return NotFound();
            }

            var fineDetails = await _context.FineDetails
                .FirstOrDefaultAsync(m => m.FineId == id);
            if (fineDetails == null)
            {
                return NotFound();
            }

            return View(fineDetails);
        }

        // POST: Fine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FineDetails == null)
            {
                return Problem("Entity set 'LibraryManagementSystemContext.FineDetails'  is null.");
            }
            var fineDetails = await _context.FineDetails.FindAsync(id);
            if (fineDetails != null)
            {
                _context.FineDetails.Remove(fineDetails);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FineDetailsExists(int id)
        {
          return (_context.FineDetails?.Any(e => e.FineId == id)).GetValueOrDefault();
        }
    }
}
