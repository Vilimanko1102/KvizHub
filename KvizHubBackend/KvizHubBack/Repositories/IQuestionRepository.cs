using KvizHubBack.Models;

namespace KvizHubBack.Repositories
{
    public interface IQuestionRepository
    {
        void Add(Question question);
        void Update(Question question);
        void Delete(Question question);
        Question? GetById(int id);
        IEnumerable<Question> GetAll();
        IEnumerable<Question> GetByQuizId(int quizId);
    }
}
