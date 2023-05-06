using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Areas.Identity.Data;
using LibraryManagementSystem.Models;
using Microsoft.Data.SqlClient;
using RestSharp;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace LibraryManagementSystem.Controllers
{
    public class BookDetailsController : Controller
    {
        private readonly LibraryManagementSystemContext _context;

        public BookDetailsController(LibraryManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> SearchNew(string searchString)
        {

            List<OpenLibBook> books = new();
            if (!String.IsNullOrEmpty(searchString))
            {
                var client = new RestClient("https://openlibrary.org/");
                Console.WriteLine(searchString);
                var request = new RestRequest("search.json", Method.GET);
                request.AddParameter("title", searchString);
                request.AddParameter("limit", 20);
                var response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    JObject forecastNode = JObject.Parse(response.Content)!;
                    JToken data = forecastNode.SelectToken("docs");
                    books = data.ToObject<List<OpenLibBook>>();
                    // Do something with the list of books.
                }
                else
                {
                    // Error occurred while retrieving book information.
                }
            }
            ViewBag.SearchResult = books;
            return View();
        }
        // GET: BookDetails
        public async Task<IActionResult> Index(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                return _context.BookDetails != null ?
                              View(await _context.BookDetails.Where(s => s.BookTitle.Contains(searchString)).ToListAsync()) :
                              Problem("Entity set 'LibraryManagementSystemContext.BookDetails'  is null.");
            }
            return _context.BookDetails != null ?
                          View(await _context.BookDetails.ToListAsync()) :
                          Problem("Entity set 'LibraryManagementSystemContext.BookDetails'  is null.");
        }

        // GET: BookDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BookDetails == null)
            {
                return NotFound();
            }
            var reservationDetails = await _context.ReservationDetails
                .Include(d => d.Book)
                .Include(d => d.ReservedUser)
                .Where(d => d.Book.BookId == id).ToListAsync();
            ViewBag.ReservationDetails = reservationDetails;
            var bookDetails = await _context.BookDetails
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (bookDetails == null)
            {
                return NotFound();
            }

            return View(bookDetails);
        }

        // GET: BookDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,BookTitle,Category,Author,Publication,BookEdition,Price,RackNo")] BookDetails bookDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookDetails);
        }

        // GET: BookDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BookDetails == null)
            {
                return NotFound();
            }

            var bookDetails = await _context.BookDetails.FindAsync(id);
            if (bookDetails == null)
            {
                return NotFound();
            }
            return View(bookDetails);
        }

        // POST: BookDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,BookTitle,Category,Author,Publication,BookEdition,Price,RackNo")] BookDetails bookDetails)
        {
            if (id != bookDetails.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookDetailsExists(bookDetails.BookId))
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
            return View(bookDetails);
        }

        // GET: BookDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookDetails == null)
            {
                return NotFound();
            }

            var bookDetails = await _context.BookDetails
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (bookDetails == null)
            {
                return NotFound();
            }

            return View(bookDetails);
        }

        // POST: BookDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BookDetails == null)
            {
                return Problem("Entity set 'LibraryManagementSystemContext.BookDetails'  is null.");
            }
            var bookDetails = await _context.BookDetails.FindAsync(id);
            if (bookDetails != null)
            {
                _context.BookDetails.Remove(bookDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookDetailsExists(int id)
        {
            return (_context.BookDetails?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
