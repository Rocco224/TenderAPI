using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TenderAPI.Contexts;
using TenderAPI.Models;

namespace TenderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticesController : ControllerBase
    {
        private readonly TenderDbContext _context;

        public PracticesController(TenderDbContext context)
        {
            _context = context;
        }

        // GET: api/Practices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Practice>>> GetPractices()
        {
            try
            {
                if (_context.Practices == null)
                {
                    return NotFound();
                }

                return await _context.Practices.ToListAsync();
               
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("ExpiringPractices")]
        public async Task<ActionResult<IEnumerable<ExpiringPractice>>> GetExpiringPractices()
        {
            try
            {
                if (_context.Practices == null)
                {
                    return NotFound();
                }

                var expiringPractices = await _context.ExpiringPractices.ToListAsync();

                if(expiringPractices.Count == 0)
                {
                    return NotFound();
                }

                return expiringPractices;
            }
            catch (Exception)
            {

                throw;
            }
        }


        // GET: api/Practices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Practice>> GetPractice(int id)
        {
            try
            {
                if (_context.Practices == null)
                {
                    return NotFound();
                }
                var practice = await _context.Practices.FindAsync(id);

                if (practice == null)
                {
                    return NotFound();
                }

                return practice;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT: api/Practices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPractice(int id, Practice practice)
        {
            try
            {
                if (id != practice.PracticeId)
                {
                    return BadRequest();
                }

                _context.Entry(practice).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PracticeExists(id))
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
            catch (Exception)
            {

                throw;
            }
        }

        // POST: api/Practices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Practice>> PostPractice(Practice practice)
        {
            try
            {
                if (_context.Practices == null)
                {
                    return Problem("Entity set 'TenderDbContext.Practices'  is null.");
                }
                _context.Practices.Add(practice);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (PracticeExists(practice.PracticeId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtAction("GetPractice", new { id = practice.PracticeId }, practice);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // DELETE: api/Practices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePractice(int id)
        {
            try
            {
                if (_context.Practices == null)
                {
                    return NotFound();
                }
                var practice = await _context.Practices.FindAsync(id);
                if (practice == null)
                {
                    return NotFound();
                }

                _context.Practices.Remove(practice);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool PracticeExists(int id)
        {
            return (_context.Practices?.Any(e => e.PracticeId == id)).GetValueOrDefault();
        }
    }
}
