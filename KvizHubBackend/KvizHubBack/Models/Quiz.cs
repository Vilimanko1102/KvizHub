namespace KvizHubBack.Models
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public Difficulty Difficulty { get; set; } = Difficulty.Medium;
        public int TimeLimit { get; set; } // in minutes
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int CreatedBy { get; set; } // FK to User

        public ICollection<Question>? Questions { get; set; }
        public ICollection<QuizAttempt>? QuizAttempts { get; set; }
    }
}
