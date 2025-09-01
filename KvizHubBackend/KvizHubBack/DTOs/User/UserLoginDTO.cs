using System.ComponentModel.DataAnnotations;

namespace KvizHubBack.DTOs.User
{
    public class UserLoginDto
    {
        [Required]
        public string UsernameOrEmail { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
