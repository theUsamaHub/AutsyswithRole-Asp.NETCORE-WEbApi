using Authsyswithrole.Data;
using Authsyswithrole.DTOs;
using Authsyswithrole.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authsyswithrole.Controllers
{
    // Marks this class as an API controller
    [ApiController]

    // Base route → api/roles
    [Route("api/[controller]")]

    // Entire controller accessible only by users with Admin role
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Dependency Injection of database context
        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        // ---------------------------------------------------------
        // GET: api/roles
        // Returns all roles from database
        // Only Admin can access because of controller-level Authorize
        // ---------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            return Ok(roles);
        }

        // ---------------------------------------------------------
        // GET: api/roles/{id}
        // Returns a single role by its ID
        // ---------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound("Role not found");
            }

            return Ok(role);
        }

        // ---------------------------------------------------------
        // POST: api/roles
        // Creates a new role
        // Example body:
        // {
        //     "name": "Manager"
        // }
        // ---------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDto dto)
        {
            // Check if role already exists
            var exists = await _context.Roles
                .AnyAsync(r => r.RoleName == dto.RoleName);

            if (exists)
            {
                return BadRequest("Role already exists");
            }

            // Create role entity
            var role = new Role
            {
                RoleName = dto.RoleName
            };

            // Add role to database
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return Ok(role);
        }

        // ---------------------------------------------------------
        // PUT: api/roles/{id}
        // Updates an existing role
        // ---------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, RoleDto dto)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound("Role not found");
            }

            // Update role name
            role.RoleName = dto.RoleName;

            // Save changes to database
            await _context.SaveChangesAsync();

            return Ok(role);
        }

        // ---------------------------------------------------------
        // DELETE: api/roles/{id}
        // Deletes a role from database
        // ---------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound("Role not found");
            }

            // Remove role from database
            _context.Roles.Remove(role);

            await _context.SaveChangesAsync();

            return Ok("Role deleted successfully");
        }
    }
}