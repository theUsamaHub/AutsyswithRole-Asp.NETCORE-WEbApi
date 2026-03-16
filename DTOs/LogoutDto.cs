using System.ComponentModel.DataAnnotations;

namespace Authsyswithrole.DTOs
{
    public class LogoutDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}