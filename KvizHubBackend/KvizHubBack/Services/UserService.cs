using KvizHubBack.DTOs.User;
using KvizHubBack.Models;
using KvizHubBack.Repositories;
using System.Collections.Generic;
using System.Linq;
using BCrypt.Net;

namespace KvizHubBack.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        // ========================
        // Registracija korisnika
        // ========================
        public UserDto Register(UserRegisterDto dto)
        {
            if (_repo.GetByUsername(dto.Username) != null)
                throw new Exception("Username already exists");

            if (_repo.GetByEmail(dto.Email) != null)
                throw new Exception("Email already exists");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                AvatarUrl = dto.AvatarUrl,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _repo.Add(user);

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl
            };
        }

        // ========================
        // Login korisnika
        // ========================
        public UserDto Login(UserLoginDto dto)
        {
            var user = _repo.GetByUsernameOrEmail(dto.UsernameOrEmail);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid username or password");

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl
            };
        }

        // ========================
        // Dohvatanje korisnika po ID
        // ========================
        public UserDto GetById(int id)
        {
            var user = _repo.GetById(id) ?? throw new Exception("User not found");

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl
            };
        }

        // ========================
        // Dohvatanje korisnika po username
        // ========================
        public UserDto GetByUsername(string username)
        {
            var user = _repo.GetByUsername(username) ?? throw new Exception("User not found");

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl
            };
        }

        // ========================
        // Dohvatanje svih korisnika
        // ========================
        public IEnumerable<UserDto> GetAll()
        {
            return _repo.GetAll()
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    AvatarUrl = u.AvatarUrl
                });
        }

        // ========================
        // Update korisnika
        // ========================
        public void Update(int id, UserUpdateDto dto)
        {
            var user = _repo.GetById(id) ?? throw new Exception("User not found");

            if (!string.IsNullOrEmpty(dto.Username))
                user.Username = dto.Username;

            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            if (!string.IsNullOrEmpty(dto.AvatarUrl))
                user.AvatarUrl = dto.AvatarUrl;

            _repo.Update(user);
        }

        // ========================
        // Brisanje korisnika
        // ========================
        public void Delete(int id)
        {
            var user = _repo.GetById(id) ?? throw new Exception("User not found");
            _repo.Delete(user);
        }

        // ========================
        // Dohvatanje istorije kvizova korisnika
        // ========================
        public IEnumerable<UserQuizResultDto> GetUserQuizHistory(int userId)
        {
            var attempts = _repo.GetUserQuizAttempts(userId);

            return attempts.Select(a => new UserQuizResultDto
            {
                QuizId = a.QuizId,
                QuizTitle = a.Quiz?.Title ?? "",
                Score = a.Score,
                Percentage = a.Percentage,
                TotalQuestions = a.UserAnswers?.Count ?? 0,
                CorrectAnswers = a.UserAnswers?.Count(ua => ua.IsCorrect) ?? 0,
                StartedAt = a.StartedAt,
                FinishedAt = a.FinishedAt,
                TimeSpent = a.TimeSpent
            });
        }
    }
}
