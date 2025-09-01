using KvizHubBack.Data;
using KvizHubBack.Models;
using Microsoft.EntityFrameworkCore;
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
            return _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefault(q => q.Id == id)!;
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
