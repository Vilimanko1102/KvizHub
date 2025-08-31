using KvizHubBack.Data;
using KvizHubBack.Models;
using System.Linq;

namespace KvizHubBack.Repositories
{
    public class UserRepository
    {
        private readonly KvizHubContext _context;

        public UserRepository(KvizHubContext context)
        {
            _context = context;
        }

        public bool ExistsByUsername(string username) => _context.Users.Any(u => u.Username == username);

        public User GetByUsername(string username) => _context.Users.FirstOrDefault(u => u.Username == username);

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public IQueryable<User> GetAll() => _context.Users;
    }
}
