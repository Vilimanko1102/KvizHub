namespace KvizHubBack.DTOs.QuizAttempt
{
    public class QuizAttemptUpdateDto
    {
        public int Score { get; set; }
        public float Percentage { get; set; }
        public int TimeSpent { get; set; } // in seconds
    }
}
