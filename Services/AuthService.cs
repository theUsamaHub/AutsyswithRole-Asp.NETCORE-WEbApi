using Authsyswithrole.Data;
using Authsyswithrole.DTOs;
using Authsyswithrole.Models;
using Microsoft.EntityFrameworkCore;
namespace Authsyswithrole.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwt;

        public AuthService(AppDbContext context, JwtService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        public async Task<string> Register(RegisterDto dto)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = hash,
                RoleId = dto.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user = await _context.Users.Include(u => u.Role)
                                       .FirstAsync(u => u.Email == dto.Email);

            return _jwt.GenerateToken(user);
        }

        public async Task<string> Login(LoginDto dto)
        {
            var user = await _context.Users
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
                return null;

            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!valid)
                return null;

            return _jwt.GenerateToken(user);
        }
    }
}
