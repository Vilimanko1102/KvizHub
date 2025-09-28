using KvizHubBack.Data;
using KvizHubBack.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
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

        // ========================
        // Dodavanje pitanja
        // ========================
        public void Add(Question question)
        {
            _context.Questions.Add(question);
            _context.SaveChanges();
        }

        // ========================
        // Ažuriranje pitanja
        // ========================
        public void Update(Question question)
        {
            _context.Questions.Update(question);
            _context.SaveChanges();
        }

        // ========================
        // Brisanje pitanja
        // ========================
        public void Delete(Question question)
        {
            _context.Questions.Remove(question);
            _context.SaveChanges();
        }

        // ========================
        // Dohvatanje pitanja po ID sa odgovorima
        // ========================
        public Question? GetById(int id)
        {
            return _context.Questions
                .Include(q => q.Answers)  // Eager load odgovora
                .Where(q => q.Id == id).ToList()[0];
        }

        // ========================
        // Dohvatanje svih pitanja sa odgovorima
        // ========================
        public IEnumerable<Question> GetAll()
        {
            return _context.Questions
                .Include(q => q.Answers)  // Učitaj odgovore
                .ToList();
        }

        // ========================
        // Dohvatanje pitanja po kvizu sa odgovorima
        // ========================
        public IEnumerable<Question> GetByQuizId(int quizId)
        {
            return _context.Questions
                .Where(q => q.QuizId == quizId)
                .Include(q => q.Answers)  // Učitaj odgovore
                .ToList();
        }
    }
}
