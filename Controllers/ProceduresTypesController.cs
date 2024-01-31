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
    public class ProceduresTypesController : ControllerBase
    {
        private readonly TenderDbContext _context;

        public ProceduresTypesController(TenderDbContext context)
        {
            _context = context;
        }

        // GET: api/ProceduresTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProceduresType>>> GetProceduresTypes()
        {
          if (_context.ProceduresTypes == null)
          {
              return NotFound();
          }
            return await _context.ProceduresTypes.ToListAsync();
        }

        // GET: api/ProceduresTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProceduresType>> GetProceduresType(int id)
        {
          if (_context.ProceduresTypes == null)
          {
              return NotFound();
          }
            var proceduresType = await _context.ProceduresTypes.FindAsync(id);

            if (proceduresType == null)
            {
                return NotFound();
            }

            return proceduresType;
        }

        // PUT: api/ProceduresTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProceduresType(int id, ProceduresType proceduresType)
        {
            if (id != proceduresType.ProcedureTypeId)
            {
                return BadRequest();
            }

            _context.Entry(proceduresType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProceduresTypeExists(id))
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

        // POST: api/ProceduresTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProceduresType>> PostProceduresType(ProceduresType proceduresType)
        {
          if (_context.ProceduresTypes == null)
          {
              return Problem("Entity set 'TenderDbContext.ProceduresTypes'  is null.");
          }
            _context.ProceduresTypes.Add(proceduresType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProceduresType", new { id = proceduresType.ProcedureTypeId }, proceduresType);
        }

        // DELETE: api/ProceduresTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProceduresType(int id)
        {
            if (_context.ProceduresTypes == null)
            {
                return NotFound();
            }
            var proceduresType = await _context.ProceduresTypes.FindAsync(id);
            if (proceduresType == null)
            {
                return NotFound();
            }

            _context.ProceduresTypes.Remove(proceduresType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProceduresTypeExists(int id)
        {
            return (_context.ProceduresTypes?.Any(e => e.ProcedureTypeId == id)).GetValueOrDefault();
        }
    }
}
