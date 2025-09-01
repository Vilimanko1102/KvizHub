using KvizHubBack.Data;
using KvizHubBack.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace KvizHubBack.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly KvizHubContext _context;

        public AnswerRepository(KvizHubContext context)
        {
            _context = context;
        }

        public Answer GetById(int id)
        {
            var answers = _context.Answers
                            .FromSqlRaw(@"SELECT * FROM ""Answers"" WHERE ""Id"" = :id AND ROWNUM = 1",
                                new OracleParameter("id", id))
                            .AsEnumerable();   // materializes the query without EF appending FETCH

            var answer = answers.FirstOrDefault();

            return answer;

        }


        public IEnumerable<Answer> GetAll() => _context.Answers.ToList();

        public void Add(Answer answer)
        {
            _context.Answers.Add(answer);
            _context.SaveChanges();
        }

        public void Update(Answer answer)
        {
            _context.Answers.Update(answer);
            _context.SaveChanges();
        }

        public void Delete(Answer answer)
        {
            _context.Answers.Remove(answer);
            _context.SaveChanges();
        }
    }
}
