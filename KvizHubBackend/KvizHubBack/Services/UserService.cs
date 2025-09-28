using BCrypt.Net;
using KvizHubBack.DTOs.User;
using KvizHubBack.Models;
using KvizHubBack.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace KvizHubBack.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ========================
        // Registracija korisnika
        // ========================
        public UserAuthResponseDto Register(UserRegisterDto dto)
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
                CreatedAt = DateTime.Now,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _repo.Add(user);

            // Vraćamo UserAuthResponseDto
            return new UserAuthResponseDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    AvatarUrl = user.AvatarUrl
                },
                Token = GenerateJwtToken(user),
                Expiration = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]))
            };
        }

        // ========================
        // Login korisnika
        // ========================
        public UserAuthResponseDto Login(UserLoginDto dto)
        {
            var user = _repo.GetByUsernameOrEmail(dto.UsernameOrEmail);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid username or password");

            // Vraćamo UserAuthResponseDto
            return new UserAuthResponseDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    AvatarUrl = user.AvatarUrl,
                    Role = user.Role.ToString()
                },
                Token = GenerateJwtToken(user),
                Expiration = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]))
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
                AvatarUrl = user.AvatarUrl,
                Role = user.Role.ToString(),
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
