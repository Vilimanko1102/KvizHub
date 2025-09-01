using KvizHubBack.DTOs.Quiz;

namespace KvizHubBack.Services
{
    public interface IQuizService
    {
        QuizDto Create(QuizCreateDto dto);
        QuizDto Update(int id, QuizUpdateDto dto);
        void Delete(int id);
        QuizDto GetById(int id);
        IEnumerable<QuizDto> GetAll();
        IEnumerable<QuizDto> Filter(string? category, string? difficulty, string? search);
        IEnumerable<QuizResultDto> GetQuizResults(int quizId);
    }
}
