namespace KvizHubBack.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
