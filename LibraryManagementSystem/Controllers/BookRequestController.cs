using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Areas.Identity.Data;
using LibraryManagementSystem.Models;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Controllers
{
    public class BookRequestController : Controller
    {
        private readonly LibraryManagementSystemContext _context;
        private readonly UserManager<LMSUser> _userManager;

        public BookRequestController(LibraryManagementSystemContext context,UserManager<LMSUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (bookRequestModel == null)
            {
                return NotFound();
            }

            return View(bookRequestModel);
        }

        // GET: BookRequest/Create
        public async Task<IActionResult> Create(string id)
        {
            Console.WriteLine(id);
            // build the API request URL
            string apiUrl = $"https://openlibrary.org/api/books?bibkeys=ISBN:{id}&format=json&jscmd=data";

            // send the API request and get the response
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // parse the JSON response and extract the book details
            JsonElement responseJson = JsonDocument.Parse(responseBody).RootElement;
            JsonElement bookJson = responseJson.GetProperty($"ISBN:{id}");
            string bookName = bookJson.GetProperty("title").GetString();
            string publisher = bookJson.GetProperty("publishers")[0].GetProperty("name").GetString();
            string authorName = bookJson.GetProperty("authors")[0].GetProperty("name").GetString();
            string coverUrl = bookJson.GetProperty("cover").GetProperty("medium").GetString();
            ViewBag.BookName = bookName;
            ViewBag.Publisher = publisher;
            ViewBag.Author = authorName;
            // Extract Id from url
            string[] urlParts = coverUrl.Split('/');
            string coverIdWithExtension = urlParts[urlParts.Length - 1];
            string[] coverIdParts = coverIdWithExtension.Split('-');
            string coverId = coverIdParts[0];
            ViewBag.CoverId = Int32.Parse(coverId);
            // display the book details
            Console.WriteLine($"Book name: {bookName}");
            Console.WriteLine($"Publisher: {publisher}");
            Console.WriteLine($"Author name: {authorName}");
            Console.WriteLine($"Cover : {coverUrl}");
            return View();
        }

        // POST: BookRequest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsAdded,BookId,BookTitle,Category,Author,Publication,BookEdition,Price,RackNo,CoverId")] BookRequestModel bookRequestModel, int id)
        {
            try
            {
                bookRequestModel.RequestedBy = await _userManager.GetUserAsync(User);
                _context.Add(bookRequestModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id,BookRequestModel bookRequestModel)
        {
            if (id != bookRequestModel.RequestId)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(bookRequestModel);
                if(bookRequestModel.IsAdded == true)
                {
                    // Find a way to map two class
                    BookDetails newBook = new();
                    //var newBk = Mapper.Map<BookRequestModel, BookDetails>(bookRequestModel);
                    newBook.BookTitle = bookRequestModel.BookTitle;
                    newBook.Category = bookRequestModel.Category;
                    newBook.Author = bookRequestModel.Author;
                    newBook.Publication = bookRequestModel.Publication;
                    newBook.BookEdition = bookRequestModel.BookEdition;
                    newBook.Price = bookRequestModel.Price;
                    newBook.RackNo = bookRequestModel.RackNo;
                    newBook.CoverId = bookRequestModel.CoverId;
                    _context.BookDetails.Add(newBook);
                }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookRequestModelExists(bookRequestModel.RequestId))
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

        // GET: BookRequest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookRequestModel == null)
            {
                return NotFound();
            }

            var bookRequestModel = await _context.BookRequestModel
                .FirstOrDefaultAsync(m => m.RequestId == id);
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
            return (_context.BookRequestModel?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }
    }
}
