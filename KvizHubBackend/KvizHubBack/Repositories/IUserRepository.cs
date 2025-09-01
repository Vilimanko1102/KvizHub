using KvizHubBack.Models;

namespace KvizHubBack.Repositories
{
    public interface IUserRepository
    {
        User GetById(int id);
        User GetByUsername(string username);
        void Add(User user);
        void Update(User user);
        void Delete(User user);
        IEnumerable<User> GetAll();
    }
}
