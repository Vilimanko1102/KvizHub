using KvizHubBack.Data;
using KvizHubBack.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;

namespace KvizHubBack.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly KvizHubContext _context;

        public QuizRepository(KvizHubContext context)
        {
            _context = context;
        }

        public void Add(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            _context.SaveChanges();
        }

        public void Update(Quiz quiz)
        {
            _context.Quizzes.Update(quiz);
            _context.SaveChanges();
        }

        public void Delete(Quiz quiz)
        {
            _context.Quizzes.Remove(quiz);
            _context.SaveChanges();
        }

        public Quiz? GetById(int id)
        {
            return _context.Quizzes
                .FromSqlRaw(@"SELECT * FROM ""Quizzes"" WHERE ""Id"" = :id AND ROWNUM = 1",
                    new OracleParameter("id", id))
                .AsEnumerable()
                .FirstOrDefault();
        }

        public IEnumerable<Quiz> GetAll()
        {
            return _context.Quizzes.ToList();
        }

        public IEnumerable<Quiz> Filter(string? category, string? difficulty, string? search)
        {
            var query = _context.Quizzes.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(q => q.Category == category);

            if (!string.IsNullOrEmpty(difficulty))
                query = query.Where(q => q.Difficulty.ToString() == difficulty);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(q => q.Title.Contains(search) || q.Description.Contains(search));

            return query.ToList();
        }

        

        public IEnumerable<QuizAttempt> GetQuizResults(int quizId)
        {
            return _context.QuizAttempts
                .Where(qa => qa.QuizId == quizId && qa.FinishedAt != null)
                .Include(qa => qa.User)
                .ToList();
        }

        public IEnumerable<Quiz> GetAllWithQuestions()
        {
            return _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers) // <--- OVO JE BITNO
                .ToList();
        }

        public Quiz? GetByIdWithQuestions(int id)
        {
            return _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers) // <--- OVO JE BITNO
                .Where(q => q.Id == id).ToList()[0];
        }

    }
}
