using KvizHubBack.Data;
using KvizHubBack.Models;

namespace KvizHubBack.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly KvizHubContext _context;

        public QuestionRepository(KvizHubContext context)
        {
            _context = context;
        }

        public Question GetById(int id) => _context.Questions.FirstOrDefault(q => q.Id == id);

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
