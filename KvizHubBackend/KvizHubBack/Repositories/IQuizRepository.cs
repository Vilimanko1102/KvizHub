using KvizHubBack.Models;

namespace KvizHubBack.Repositories
{
    public interface IQuizRepository
    {
        Quiz GetById(int id);
        IEnumerable<Quiz> GetAll();
        void Add(Quiz quiz);
        void Update(Quiz quiz);
        void Delete(Quiz quiz);
    }
}
