namespace KvizHubBack.Models
{
    public class QuizAttempt
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
        public float Percentage { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FinishedAt { get; set; }
        public int TimeSpent { get; set; } // in seconds

        public Quiz? Quiz { get; set; }
        public User? User { get; set; }
        public ICollection<UserAnswer>? UserAnswers { get; set; }
    }
}
