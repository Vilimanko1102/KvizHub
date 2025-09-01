using KvizHubBack.Data;
using KvizHubBack.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;

namespace KvizHubBack.Repositories
{
    public class UserAnswerRepository : IUserAnswerRepository
    {
        private readonly KvizHubContext _context;

        public UserAnswerRepository(KvizHubContext context)
        {
            _context = context;
        }

        public void Add(UserAnswer answer)
        {
            _context.UserAnswers.Add(answer);
            _context.SaveChanges();
        }

        public void Update(UserAnswer answer)
        {
            _context.UserAnswers.Update(answer);
            _context.SaveChanges();
        }

        public void Delete(UserAnswer answer)
        {
            _context.UserAnswers.Remove(answer);
            _context.SaveChanges();
        }

        public UserAnswer? GetById(int id)
        {
            var userAnswers = _context.UserAnswers
                .FromSqlRaw(@"SELECT * FROM ""UserAnswers"" WHERE ""Id"" = :id AND ROWNUM = 1",
                    new OracleParameter("id", id))
                .AsEnumerable();
            return userAnswers.FirstOrDefault();
        }

        public IEnumerable<UserAnswer> GetByQuizAttempt(int quizAttemptId)
        {
            return _context.UserAnswers
                .Where(ua => ua.QuizAttemptId == quizAttemptId)
                .Include(ua => ua.Question)
                .ToList();
        }
    }
}
