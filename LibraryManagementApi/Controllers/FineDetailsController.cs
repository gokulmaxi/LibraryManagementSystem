using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Areas.Identity.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FineDetailsController : ControllerBase
    {
        private readonly LibraryManagementSystemContext _context;

        public FineDetailsController(LibraryManagementSystemContext context)
        {
            _context = context;
        }

        // GET: api/FineDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FineDetails>>> GetFineDetails()
        {
          if (_context.FineDetails == null)
          {
              return NotFound();
          }
            return await _context.FineDetails
                .Include(d => d.Reservation.ReservedUser)
                .Include(d => d.Reservation.Book)
                .Include(d => d.Reservation)
                .ToListAsync();
        }

        // GET: api/FineDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FineDetails>> GetFineDetails(string id)
        {
          if (_context.FineDetails == null)
          {
              return NotFound();
          }
            var fineDetails = await _context.FineDetails
                .Include(d => d.Reservation)
                .Where(d=> d.Reservation.ReservedUser.Id == id)
                .FirstOrDefaultAsync();

            if (fineDetails == null)
            {
                return NotFound();
            }

            return fineDetails;
        }

        // PUT: api/FineDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFineDetails(int id, bool paid)
        {
            var fineDetails = await _context.FineDetails.Where(d => d.FineId == id).FirstOrDefaultAsync();
            fineDetails.FineId = id;
            fineDetails.Paid = paid;
            _context.Entry(fineDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FineDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FineDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FineDetails>> PostFineDetails(FineDetails fineDetails)
        {
          if (_context.FineDetails == null)
          {
              return Problem("Entity set 'LibraryManagementSystemContext.FineDetails'  is null.");
          }
            _context.FineDetails.Add(fineDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFineDetails", new { id = fineDetails.FineId }, fineDetails);
        }

        // DELETE: api/FineDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFineDetails(int id)
        {
            if (_context.FineDetails == null)
            {
                return NotFound();
            }
            var fineDetails = await _context.FineDetails.FindAsync(id);
            if (fineDetails == null)
            {
                return NotFound();
            }

            _context.FineDetails.Remove(fineDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FineDetailsExists(int id)
        {
            return (_context.FineDetails?.Any(e => e.FineId == id)).GetValueOrDefault();
        }
    }
}
