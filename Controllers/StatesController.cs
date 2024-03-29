﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TenderAPI.Data;
using TenderAPI.Models;

namespace TenderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly TenderDbContext _context;

        public StatesController(TenderDbContext context)
        {
            _context = context;
        }

        // GET: api/States
        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {
            try
            {
                if (_context.States == null)
                {
                    return NotFound();
                }
                return await _context.States.Include(customer => customer.Practices).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: api/States/5
        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetState(int id)
        {
            try
            {
                if (_context.States == null)
                {
                    return NotFound();
                }
                var state = await _context.States.FindAsync(id);

                if (state == null)
                {
                    return NotFound();
                }

                return state;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT: api/States/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutState(int id, State state)
        {
            try
            {
                if (id != state.StateId)
                {
                    return BadRequest();
                }

                _context.Entry(state).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StateExists(id))
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

        // POST: api/States
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<State>> PostState(State state)
        {
            try
            {
                if (_context.States == null)
                {
                    return Problem("Entity set 'TenderDbContext.States'  is null.");
                }
                _context.States.Add(state);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetState", new { id = state.StateId }, state);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // DELETE: api/States/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            try
            {
                if (_context.States == null)
                {
                    return NotFound();
                }
                var state = await _context.States.FindAsync(id);
                if (state == null)
                {
                    return NotFound();
                }

                _context.States.Remove(state);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool StateExists(int id)
        {
            return (_context.States?.Any(e => e.StateId == id)).GetValueOrDefault();
        }
    }
}
