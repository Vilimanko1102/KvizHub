namespace KvizHubBack.DTOs.Quiz
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int? TimeLimit { get; set; }
        public int QuestionCount { get; set; }

        // Novo polje
        public bool IsPlayable { get; set; }
    }

}
