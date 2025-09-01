namespace KvizHubBack.DTOs.Question
{
    public class QuestionCreateDto
    {
        public int QuizId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Points { get; set; } = 1;
    }
}
