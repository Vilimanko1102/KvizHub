using KvizHubBack.Models;

namespace KvizHubBack.Repositories
{
    public interface IQuizRepository
    {
        void Add(Quiz quiz);
        void Update(Quiz quiz);
        void Delete(Quiz quiz);
        Quiz? GetById(int id);
        IEnumerable<Quiz> GetAll();
        IEnumerable<Quiz> Filter(string? category, string? difficulty, string? search);
        IEnumerable<QuizAttempt> GetQuizResults(int quizId);
    }
}
