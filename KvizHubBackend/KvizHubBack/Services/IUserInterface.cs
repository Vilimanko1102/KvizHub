using KvizHubBack.DTOs.User;

namespace KvizHubBack.Services
{
    public interface IUserService
    {
        UserDto Register(UserRegisterDto dto);
        UserDto Login(UserLoginDto dto);
        IEnumerable<UserDto> GetAll();
    }
}
