using System.ComponentModel.DataAnnotations;

namespace KvizHubBack.DTOs.User
{
    public class UserUpdateDto
    {
        [StringLength(50, MinimumLength = 3)]
        public string? Username { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }

        public string? AvatarUrl { get; set; }
    }
}
