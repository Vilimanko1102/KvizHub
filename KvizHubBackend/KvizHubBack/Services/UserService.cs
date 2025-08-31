using KvizHubBack.DTOs.User;
using KvizHubBack.Models;
using KvizHubBack.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace KvizHubBack.Services
{
    public class UserService
    {
        private readonly UserRepository _repo;

        public UserService(UserRepository repo)
        {
            _repo = repo;
        }

        public UserDto Register(UserRegisterDto dto)
        {
            if (_repo.ExistsByUsername(dto.Username))
                throw new System.Exception("Username already exists.");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _repo.Add(user);

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public UserDto Login(UserLoginDto dto)
        {
            var user = _repo.GetByUsername(dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new System.Exception("Invalid username or password.");

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public List<UserDto> GetAll()
        {
            return _repo.GetAll()
                        .Select(u => new UserDto
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Email = u.Email
                        }).ToList();
        }
    }
}
