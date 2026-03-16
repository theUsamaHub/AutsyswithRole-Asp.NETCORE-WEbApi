using System.ComponentModel.DataAnnotations;

namespace Authsyswithrole.DTOs
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}