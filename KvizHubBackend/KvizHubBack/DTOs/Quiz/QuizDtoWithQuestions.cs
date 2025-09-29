using KvizHubBack.DTOs.Question;

namespace KvizHubBack.DTOs.Quiz
{
    public class QuizDtoWithQuestions
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public string Category { get; set; }
        public int TimeLimit { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
    }
}
