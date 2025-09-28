namespace KvizHubBack.Models
{
    public enum QuestionType
    {
        SingleChoice,
        MultipleChoice,
        TrueFalse,
        FillIn
    }
    public class Question
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Text { get; set; } = null!;
        public QuestionType Type { get; set; }
        public int Points { get; set; }
        public Quiz? Quiz { get; set; }
        public ICollection<Answer>? Answers { get; set; }
        public ICollection<UserAnswer>? UserAnswers { get; set; }
    }
}
