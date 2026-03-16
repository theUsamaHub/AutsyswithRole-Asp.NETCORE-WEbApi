using Authsyswithrole.Data;
using Authsyswithrole.DTOs;
using Authsyswithrole.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authsyswithrole.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class AuthController : ControllerBase
    {
        private readonly AuthService _auth;
        private readonly AppDbContext _context;

        public AuthController(AuthService auth,AppDbContext context)
        {
            _auth = auth;
            _context = context;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(RegisterDto dto)
        //{
        //    var token = await _auth.Register(dto);
        //    return Ok(new { token });
        //}
        //added logic
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var result = await _auth.Register(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _auth.Login(dto);

            if (token == null)
                return Unauthorized();

            return Ok(new { token });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            if (email == null)
                return Unauthorized();

            var user = await _context.Users
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.RoleId,
                Role = user.Role.RoleName
            });
        }
        //[HttpGet("users")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users = await _context.Users
        //        .Include(u => u.Role)
        //        .Select(u => new
        //        {
        //            u.Id,
        //            u.Username,
        //            u.Email,
        //            u.RoleId,
        //            Role = u.Role.RoleName
        //        })
        //        .ToListAsync();

        //    return Ok(users);
        //}
        //[HttpGet("users/{id}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> GetUserById(int id)
        //{
        //    var user = await _context.Users
        //        .Include(u => u.Role)
        //        .Where(u => u.Id == id)
        //        .Select(u => new
        //        {
        //            u.Id,
        //            u.Username,
        //            u.Email,
        //            u.RoleId,
        //            Role = u.Role.RoleName
        //        })
        //        .FirstOrDefaultAsync();

        //    if (user == null)
        //        return NotFound(new { message = "User not found" });

        //    return Ok(user);
        //}
    }
}
