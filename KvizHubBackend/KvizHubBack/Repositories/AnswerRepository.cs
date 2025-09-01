using KvizHubBack.Data;
using KvizHubBack.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;

namespace KvizHubBack.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly KvizHubContext _context;

        public AnswerRepository(KvizHubContext context)
        {
            _context = context;
        }

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

        public Answer? GetById(int id)
        {
            var answers = _context.Answers
                .FromSqlRaw(@"SELECT * FROM ""Answers"" WHERE ""Id"" = :id AND ROWNUM = 1",
                    new OracleParameter("id", id))
                .AsEnumerable();
            return answers.FirstOrDefault();
        }

        public IEnumerable<Answer> GetByQuestionId(int questionId)
        {
            return _context.Answers
                .Where(a => a.QuestionId == questionId)
                .ToList();
        }
    }
}
