using KvizHubBack.Data;
using KvizHubBack.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KvizHubBack.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly KvizHubContext _context;

        public QuestionRepository(KvizHubContext context)
        {
            _context = context;
        }

        public void Add(Question question)
        {
            _context.Questions.Add(question);
            _context.SaveChanges();
        }

        public void Update(Question question)
        {
            _context.Questions.Update(question);
            _context.SaveChanges();
        }

        public void Delete(Question question)
        {
            _context.Questions.Remove(question);
            _context.SaveChanges();
        }

        public Question? GetById(int id)
        {
            return _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefault(q => q.Id == id);
        }

        public IEnumerable<Question> GetAll()
        {
            return _context.Questions
                .Include(q => q.Answers)
                .ToList();
        }

        public IEnumerable<Question> GetByQuizId(int quizId)
        {
            return _context.Questions
                .Where(q => q.QuizId == quizId)
                .Include(q => q.Answers)
                .ToList();
        }
    }
}
