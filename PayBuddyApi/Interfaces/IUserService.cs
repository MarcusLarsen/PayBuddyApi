using PayBuddyApi.DTO.User;

namespace PayBuddyApi.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> SearchUsersAsync(string searchTerm, string currentUserId);
        Task<UserDto?> GetUserByIdAsync(string userId);
    }
}
