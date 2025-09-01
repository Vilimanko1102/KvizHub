namespace KvizHubBack.DTOs.Answer
{
    public class AnswerCreateDto
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
    }
}
