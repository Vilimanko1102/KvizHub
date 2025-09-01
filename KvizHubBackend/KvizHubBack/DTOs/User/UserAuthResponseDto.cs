namespace KvizHubBack.DTOs.User
{
    public class UserAuthResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public UserDto User { get; set; } = null!;
    }
}
