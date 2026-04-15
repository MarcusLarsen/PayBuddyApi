using PayBuddyApi.DTO.Friendship;

namespace PayBuddyApi.Interfaces
{
    public interface IFriendshipService
    {
        Task<List<FriendDto>> GetFriendsAsync(string userId);
        Task<(bool Success, string Message)> AddFriendAsync(string userId, FriendForSaveDTO dto);
        Task<(bool Success, string Message)> DeleteFriendAsync(int friendshipId, string userId);
    }
}
