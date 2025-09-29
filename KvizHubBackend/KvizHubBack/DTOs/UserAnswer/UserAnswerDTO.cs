namespace KvizHubBack.DTOs.UserAnswer
{
    public class UserAnswerDto
    {
        public int Id { get; set; }
        public int QuizAttemptId { get; set; }
        public int QuestionId { get; set; }
        public List<int>? SelectedAnswerIds { get; set; }
        public string? SelectedAnswerIdsCsv { get; set; }
        public string? TextAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
