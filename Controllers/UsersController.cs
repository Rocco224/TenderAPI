using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TenderAPI.Authentication;
using TenderAPI.Contexts;
using TenderAPI.Cryptation;
using TenderAPI.Models;

namespace TenderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TenderDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(TenderDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                if (_context.Users == null)
                {
                    return NotFound();
                }
                return await _context.Users.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                if (_context.Users == null)
                {
                    return NotFound();
                }
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            try
            {
                if (id != user.UserId)
                {
                    return BadRequest();
                }

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Register")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                if (_context.Users == null)
                {
                    return Problem("Entity set 'TenderDbContext.Users'  is null.");
                }

                Dictionary<string, string> passwordInfo = PasswordHasher.HashPasswordWithPepper(user.Password);

                user.Password = passwordInfo["HashedPassword"];
                user.Salt = passwordInfo["Salt"];

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.UserId }, user);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(User user)
        {
            try
            {
                if (user.Email == null && user.Username == null)
                {
                    return BadRequest("Immettere email o username.");
                }

                var dbUser = _context.Users.FirstOrDefault(u => u.Email == user.Email || u.Username == user.Username);

                if (dbUser == null)
                {
                    return NotFound("Credenziali non valide.");
                }

                Dictionary<string, string> passwordInfo = PasswordHasher.HashPasswordWithPepper(user.Password, dbUser.Salt);

                user.Password = passwordInfo["HashedPassword"];

                if (user.Password == dbUser.Password)
                {
                    string token = JwtHandler.GenerateJwtToken(user, _configuration);

                    return Ok(new { Token = token, Message = "Login riuscito." });
                }

                return BadRequest("Credenziali non valide.");
            }

            catch (Exception)
            {
                throw;
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if (_context.Users == null)
                {
                    return NotFound();
                }
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
