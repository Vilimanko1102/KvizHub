using KvizHubBack.DTOs.UserAnswer;

namespace KvizHubBack.DTOs.QuizAttempt
{
    public class QuizAttemptDto
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string QuizTitle { get; set; } = "";
        public int UserId { get; set; }
        public int Score { get; set; }
        public float Percentage { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public int TimeSpent { get; set; }
        public IEnumerable<UserAnswerDto>? UserAnswers { get; set; }
    }
}
