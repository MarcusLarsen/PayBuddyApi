using PayBuddyApi.DTO.Auth;

namespace PayBuddyApi.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
