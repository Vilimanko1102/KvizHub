namespace KvizHubBack.DTOs.UserAnswer
{
    public class UserAnswerUpdateDto
    {
        public List<int>? SelectedAnswerIds { get; set; }
        public string? TextAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
