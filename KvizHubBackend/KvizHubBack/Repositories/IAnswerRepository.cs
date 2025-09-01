using KvizHubBack.Models;

namespace KvizHubBack.Repositories
{
    public interface IAnswerRepository
    {
        void Add(Answer answer);
        void Update(Answer answer);
        void Delete(Answer answer);
        Answer? GetById(int id);
        IEnumerable<Answer> GetByQuestionId(int questionId);
    }
}
