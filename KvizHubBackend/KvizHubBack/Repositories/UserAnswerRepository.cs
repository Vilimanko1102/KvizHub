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
            return _context.UserAnswers
                .Include(ua => ua.Question)
                .Include(ua => ua.QuizAttempt)
                .FirstOrDefault(ua => ua.Id == id);
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
