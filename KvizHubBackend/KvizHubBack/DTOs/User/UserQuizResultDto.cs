namespace KvizHubBack.DTOs.User
{
    public class UserQuizResultDto
    {
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public int Score { get; set; }
        public float Percentage { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public int TimeSpent { get; set; }
    }
}
