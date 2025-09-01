using KvizHubBack.Models;
using System.Collections.Generic;

namespace KvizHubBack.Repositories
{
    public interface IUserAnswerRepository
    {
        void Add(UserAnswer answer);
        void Update(UserAnswer answer);
        void Delete(UserAnswer answer);
        UserAnswer? GetById(int id);
        IEnumerable<UserAnswer> GetByQuizAttempt(int quizAttemptId);
    }
}
