using KvizHubBack.Data;
using KvizHubBack.Models;
using System.Linq;

namespace KvizHubBack.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly KvizHubContext _context;
        public UserRepository(KvizHubContext context) => _context = context;

        public void Add(User user) => _context.Users.Add(user);
        public void Delete(User user) => _context.Users.Remove(user);
        public IEnumerable<User> GetAll() => _context.Users.ToList();
        public User GetById(int id) => _context.Users.Find(id);
        public User GetByUsername(string username) => _context.Users.FirstOrDefault(u => u.Username == username);
        public void Update(User user) => _context.Users.Update(user);
    }
}
