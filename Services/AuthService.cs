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

        //public async Task<string> Register(RegisterDto dto)
        //{
        //    var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        //    var user = new User
        //    {
        //        Username = dto.Username,
        //        Email = dto.Email,
        //        Password = hash,
        //        RoleId = dto.RoleId
        //    };

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    user = await _context.Users.Include(u => u.Role)
        //                               .FirstAsync(u => u.Email == dto.Email);

        //    return _jwt.GenerateToken(user);
        //}
        public async Task<AuthResponseDto> Register(RegisterDto dto)
        {
            //added login for email existence and role existence

            var emailExists = await _context.Users
              .AnyAsync(x => x.Email == dto.Email);

            if (emailExists)
                throw new Exception("Email already registered");

            var roleExists = await _context.Roles
                .AnyAsync(x => x.RoleId == dto.RoleId);

            if (!roleExists)
                throw new Exception("Invalid role");

            //end 
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

            user = await _context.Users
                    .Include(u => u.Role)
                    .FirstAsync(u => u.Email == dto.Email);

            //added this logic for creating the refreshtoken
            var accessToken = _jwt.GenerateToken(user);

            var refreshToken = _jwt.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 900,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    Role = user.Role.RoleName
                }
            };
        }
        //end

        //public async Task<string> Login(LoginDto dto)
        //{
        //    var user = await _context.Users
        //                .Include(u => u.Role)
        //                .FirstOrDefaultAsync(x => x.Email == dto.Email);

        //    if (user == null)
        //        return null;

        //    bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

        //    if (!valid)
        //        return null;

        //    return _jwt.GenerateToken(user);
        //}

        public async Task<AuthResponseDto?> Login(LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
                return null;

            var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!valid)
                return null;

            var accessToken = _jwt.GenerateToken(user);
            var refreshToken = _jwt.GenerateRefreshToken();

            _context.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 900,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    Role = user.Role.RoleName
                }
            };
        }
    }
}
