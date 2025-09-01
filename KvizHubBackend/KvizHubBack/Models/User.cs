using System.Data;

namespace KvizHubBack.Models
{
    public enum Role
    {
        User,
        Admin
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public Role Role { get; set; } = Role.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<QuizAttempt>? QuizAttempts { get; set; }
    }
}
