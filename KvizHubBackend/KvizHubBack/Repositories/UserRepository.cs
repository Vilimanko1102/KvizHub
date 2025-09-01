using KvizHubBack.Data;
using KvizHubBack.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace KvizHubBack.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly KvizHubContext _context;

        public UserRepository(KvizHubContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges(); // SaveChanges odmah
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User GetById(int id)
        {
            var users = _context.Users
                .FromSqlRaw(
                    @"SELECT * FROM ""Users"" WHERE ""Id"" = :id AND ROWNUM = 1",
                    new OracleParameter("id", id))
                .AsEnumerable();

            return users.FirstOrDefault()!;
        }

        public User GetByUsername(string username)
        {
            var users = _context.Users
                .FromSqlRaw(
                    @"SELECT * FROM ""Users"" WHERE ""Username"" = :username AND ROWNUM = 1",
                    new OracleParameter("username", username))
                .AsEnumerable();

            return users.FirstOrDefault()!;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }
    }
}
