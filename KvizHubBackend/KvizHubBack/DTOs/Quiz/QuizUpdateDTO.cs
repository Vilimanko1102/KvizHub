namespace KvizHubBack.DTOs.Quiz
{
    public class QuizUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Difficulty { get; set; }
        public int? TimeLimit { get; set; }
    }
}
