using KvizHubBack.Data;
using KvizHubBack.Models;

namespace KvizHubBack.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly KvizHubContext _context;

        public AnswerRepository(KvizHubContext context)
        {
            _context = context;
        }

        public Answer GetById(int id) => _context.Answers.FirstOrDefault(a => a.Id == id);

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
