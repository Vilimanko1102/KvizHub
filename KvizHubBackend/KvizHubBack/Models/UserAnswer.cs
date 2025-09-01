namespace KvizHubBack.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int QuizAttemptId { get; set; }
        public int QuestionId { get; set; }
        public List<int>? SelectedAnswerIds { get; set; } // for multiple choice
        public string? TextAnswer { get; set; } // for FillIn
        public bool IsCorrect { get; set; }

        public QuizAttempt? QuizAttempt { get; set; }
        public Question? Question { get; set; }
    }
}
