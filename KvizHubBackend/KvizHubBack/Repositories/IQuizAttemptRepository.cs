using KvizHubBack.Models;

namespace KvizHubBack.Repositories
{
    public interface IQuizAttemptRepository
    {
        void Add(QuizAttempt attempt);
        void Update(QuizAttempt attempt);
        void Delete(QuizAttempt attempt);
        QuizAttempt? GetById(int id);
        IEnumerable<QuizAttempt> GetByUserId(int userId);
        IEnumerable<QuizAttempt> GetByQuizId(int quizId);
    }
}
