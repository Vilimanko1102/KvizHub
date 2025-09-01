using KvizHubBack.Data;
using KvizHubBack.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;

namespace KvizHubBack.Repositories
{
    public class QuizAttemptRepository : IQuizAttemptRepository
    {
        private readonly KvizHubContext _context;

        public QuizAttemptRepository(KvizHubContext context)
        {
            _context = context;
        }

        public void Add(QuizAttempt attempt)
        {
            _context.QuizAttempts.Add(attempt);
            _context.SaveChanges();
        }

        public void Update(QuizAttempt attempt)
        {
            _context.QuizAttempts.Update(attempt);
            _context.SaveChanges();
        }

        public void Delete(QuizAttempt attempt)
        {
            _context.QuizAttempts.Remove(attempt);
            _context.SaveChanges();
        }

        public QuizAttempt? GetById(int id)
        {
            return _context.QuizAttempts
                .Include(a => a.Quiz)
                .Include(a => a.UserAnswers)
                .FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<QuizAttempt> GetByUserId(int userId)
        {
            return _context.QuizAttempts
                .Include(a => a.Quiz)
                .Include(a => a.UserAnswers)
                .Where(a => a.UserId == userId)
                .ToList();
        }

        public IEnumerable<QuizAttempt> GetByQuizId(int quizId)
        {
            return _context.QuizAttempts
                .Include(a => a.UserAnswers)
                .Where(a => a.QuizId == quizId)
                .ToList();
        }
    }
}
