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
    public class BookRequestController : Controller
    {
        private readonly LibraryManagementSystemContext _context;

        public BookRequestController(LibraryManagementSystemContext context)
        {
            _context = context;
        }

        // GET: BookRequest
        public async Task<IActionResult> Index()
        {
              return _context.BookRequestModel != null ? 
                          View(await _context.BookRequestModel.ToListAsync()) :
                          Problem("Entity set 'LibraryManagementSystemContext.BookRequestModel'  is null.");
        }

        // GET: BookRequest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BookRequestModel == null)
            {
                return NotFound();
            }

            var bookRequestModel = await _context.BookRequestModel
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (bookRequestModel == null)
            {
                return NotFound();
            }

            return View(bookRequestModel);
        }

        // GET: BookRequest/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookRequest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsAdded,BookId,BookTitle,Category,Author,Publication,BookEdition,Price,RackNo,CoverId")] BookRequestModel bookRequestModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookRequestModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookRequestModel);
        }

        // GET: BookRequest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BookRequestModel == null)
            {
                return NotFound();
            }

            var bookRequestModel = await _context.BookRequestModel.FindAsync(id);
            if (bookRequestModel == null)
            {
                return NotFound();
            }
            return View(bookRequestModel);
        }

        // POST: BookRequest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IsAdded,BookId,BookTitle,Category,Author,Publication,BookEdition,Price,RackNo,CoverId")] BookRequestModel bookRequestModel)
        {
            if (id != bookRequestModel.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookRequestModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookRequestModelExists(bookRequestModel.BookId))
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
            return View(bookRequestModel);
        }

        // GET: BookRequest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookRequestModel == null)
            {
                return NotFound();
            }

            var bookRequestModel = await _context.BookRequestModel
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (bookRequestModel == null)
            {
                return NotFound();
            }

            return View(bookRequestModel);
        }

        // POST: BookRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BookRequestModel == null)
            {
                return Problem("Entity set 'LibraryManagementSystemContext.BookRequestModel'  is null.");
            }
            var bookRequestModel = await _context.BookRequestModel.FindAsync(id);
            if (bookRequestModel != null)
            {
                _context.BookRequestModel.Remove(bookRequestModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookRequestModelExists(int id)
        {
          return (_context.BookRequestModel?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
