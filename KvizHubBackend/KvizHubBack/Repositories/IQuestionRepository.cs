using KvizHubBack.Models;

namespace KvizHubBack.Repositories
{
    public interface IQuestionRepository
    {
        Question GetById(int id);
        IEnumerable<Question> GetAll();
        void Add(Question question);
        void Update(Question question);
        void Delete(Question question);
    }
}
