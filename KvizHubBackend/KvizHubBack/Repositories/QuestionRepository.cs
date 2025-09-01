using KvizHubBack.Data;
using KvizHubBack.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace KvizHubBack.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly KvizHubContext _context;

        public QuestionRepository(KvizHubContext context)
        {
            _context = context;
        }

        public Question GetById(int id)
        {
            var questions = _context.Questions
                            .FromSqlRaw(@"SELECT * FROM ""Questions"" WHERE ""Id"" = :id AND ROWNUM = 1",
                                new OracleParameter("id", id))
                            .AsEnumerable();   // materializes the query without EF appending FETCH

            var question = questions.FirstOrDefault();

            return question;

        }

        public IEnumerable<Question> GetAll() => _context.Questions.ToList();

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
    }
}
