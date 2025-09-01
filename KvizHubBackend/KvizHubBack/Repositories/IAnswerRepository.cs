using KvizHubBack.Models;

namespace KvizHubBack.Repositories
{
    public interface IAnswerRepository
    {
        Answer GetById(int id);
        IEnumerable<Answer> GetAll();
        void Add(Answer answer);
        void Update(Answer answer);
        void Delete(Answer answer);
    }
}
