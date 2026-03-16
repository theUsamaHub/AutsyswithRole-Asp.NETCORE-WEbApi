namespace Authsyswithrole.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }

        public UserInfoDto User { get; set; }
    }

    public class UserInfoDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public int RoleId { get; set; }

        public string Role { get; set; }
    }
}