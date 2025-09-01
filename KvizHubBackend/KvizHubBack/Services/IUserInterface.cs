using KvizHubBack.DTOs.User;

namespace KvizHubBack.Services
{
    public interface IUserService
    {
        UserDto Register(UserRegisterDto dto);
        UserDto Login(UserLoginDto dto);
        UserDto GetById(int id);
        UserDto GetByUsername(string username);
        IEnumerable<UserDto> GetAll();
        void Update(int id, UserUpdateDto dto);
        void Delete(int id);
        IEnumerable<UserQuizResultDto> GetUserQuizHistory(int userId);
    }
}
