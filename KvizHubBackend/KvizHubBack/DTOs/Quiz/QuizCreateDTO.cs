namespace KvizHubBack.DTOs.Quiz
{
    public class QuizCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int TimeLimit { get; set; }

        public int CreatedBy { get; set; }
    }
}
