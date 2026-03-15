using System.ComponentModel.DataAnnotations;

namespace Authsyswithrole.DTOs
{
    public class RoleDto
    {
        // Unique identifier of the role
        // Not required when creating a role but useful when returning data
        public int RoleId { get; set; }

        // Name of the role (Admin, User, Manager, etc.)
        // Required to ensure role name is always provided
        [Required]

        // Maximum length restriction to prevent oversized input
        [StringLength(50)]
        public string RoleName { get; set; }
    }
}