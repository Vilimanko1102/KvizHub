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

        // ========================
        // Dodavanje novog korisnika (registracija)
        // ========================
        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // ========================
        // Ažuriranje korisnika
        // ========================
        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        // ========================
        // Brisanje korisnika
        // ========================
        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        // ========================
        // Dohvatanje korisnika po ID
        // ========================
        public User? GetById(int id)
        {
            var users = _context.Users
                .FromSqlRaw(
                    @"SELECT * FROM ""Users"" WHERE ""Id"" = :id AND ROWNUM = 1",
                    new OracleParameter("id", id))
                .AsEnumerable();

            return users.FirstOrDefault();
        }

        // ========================
        // Dohvatanje korisnika po korisničkom imenu
        // ========================
        public User? GetByUsername(string username)
        {
            var users = _context.Users
                .FromSqlRaw(
                    @"SELECT * FROM ""Users"" WHERE ""Username"" = :username AND ROWNUM = 1",
                    new OracleParameter("username", username))
                .AsEnumerable();

            return users.FirstOrDefault();
        }

        // ========================
        // Dohvatanje korisnika po emailu
        // ========================
        public User? GetByEmail(string email)
        {
            var users = _context.Users
                .FromSqlRaw(
                    @"SELECT * FROM ""Users"" WHERE ""Email"" = :email AND ROWNUM = 1",
                    new OracleParameter("email", email))
                .AsEnumerable();

            return users.FirstOrDefault();
        }

        // ========================
        // Dohvatanje korisnika po username ili email (za login)
        // ========================
        public User? GetByUsernameOrEmail(string usernameOrEmail)
        {
            var users = _context.Users
                .FromSqlRaw(
                    @"SELECT * FROM ""Users"" 
                      WHERE ""Username"" = :ue OR ""Email"" = :ue AND ROWNUM = 1",
                    new OracleParameter("ue", usernameOrEmail))
                .AsEnumerable();

            return users.FirstOrDefault();
        }

        // ========================
        // Dohvatanje svih korisnika
        // ========================
        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        // ========================
        // Dohvatanje svih rezultata jednog korisnika
        // ========================
        public IEnumerable<QuizAttempt> GetUserQuizAttempts(int userId)
        {
            return _context.QuizAttempts
                .Where(qa => qa.UserId == userId)
                .Include(qa => qa.Quiz)
                .Include(qa => qa.UserAnswers)
                .ToList();
        }
    }
}
