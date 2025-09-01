using KvizHubBack.Models;
using System.Collections.Generic;

namespace KvizHubBack.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        void Update(User user);
        void Delete(User user);
        User? GetById(int id);
        User? GetByUsername(string username);
        User? GetByEmail(string email);
        User? GetByUsernameOrEmail(string usernameOrEmail);
        IEnumerable<User> GetAll();
        IEnumerable<QuizAttempt> GetUserQuizAttempts(int userId);
    }
}
