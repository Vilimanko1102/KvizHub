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

        public Quiz GetById(int id)
        {
            var quizzes = _context.Quizzes
                            .FromSqlRaw(@"SELECT * FROM ""Quizzes"" WHERE ""Id"" = :id AND ROWNUM = 1",
                                new OracleParameter("id", id))
                            .AsEnumerable();   // materializes the query without EF appending FETCH

            var quiz = quizzes.FirstOrDefault();

            return quiz;

        }

        public IEnumerable<Quiz> GetAll()
        {
            return _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .ToList();
        }
    }
}
