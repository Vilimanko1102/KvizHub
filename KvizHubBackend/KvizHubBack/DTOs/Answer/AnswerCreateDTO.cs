namespace KvizHubBack.DTOs.Answer
{
    public class AnswerCreateDto
    {
        public int QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
