namespace KvizHubBack.DTOs.Quiz
{
    public class QuizResultDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int Score { get; set; }
        public float Percentage { get; set; }
        public DateTime FinishedAt { get; set; }
    }
}
