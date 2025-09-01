using KvizHubBack.DTOs.Quiz;

namespace KvizHubBack.Services
{
    public interface IQuizService
    {
        QuizDto CreateQuiz(QuizCreateDto dto);
        IEnumerable<QuizDto> GetAllQuizzes();
        QuizDto GetQuizById(int id);
        QuizDto UpdateQuiz(int id, QuizUpdateDto dto);
        void DeleteQuiz(int id);
    }
}
