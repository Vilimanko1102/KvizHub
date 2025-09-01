namespace KvizHubBack.DTOs.Quiz
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty; // "Easy", "Medium", "Hard"
        public int TimeLimit { get; set; } // in minutes
        public int QuestionCount { get; set; }
    }
}
