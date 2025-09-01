namespace KvizHubBack.DTOs.UserAnswer
{
    public class UserAnswerCreateDto
    {
        public int QuizAttemptId { get; set; }
        public int QuestionId { get; set; }
        public List<int>? SelectedAnswerIds { get; set; }
        public string? TextAnswer { get; set; }
    }
}
