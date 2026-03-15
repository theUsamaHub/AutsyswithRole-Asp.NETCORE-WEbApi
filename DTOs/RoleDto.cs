using System.ComponentModel.DataAnnotations;

namespace Authsyswithrole.DTOs
{
    public class RoleDto
    {
        public int RoleId { get; set; }

        // required value
        [Required(ErrorMessage = "Role name is required")]

        // remove whitespace-only values
        [MinLength(3)]
        [MaxLength(50)]

        // restrict characters (letters only example)
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Role name must contain only letters")]

        public string RoleName { get; set; }
    }
}