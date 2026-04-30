using PayBuddyApi.DTO.Friendship;

namespace PayBuddyApi.Interfaces
{
    public interface IFriendshipService
    {
        Task<List<FriendDto>> GetFriendsAsync(string userId);
        Task<List<FriendRequestDto>> GetFriendRequestsAsync(string userId);

        Task<(bool Success, string Message)> SendFriendRequestAsync(string userId, FriendForSaveDTO dto);
        Task<(bool Success, string Message)> AcceptFriendRequestAsync(int friendshipId, string userId);
        Task<(bool Success, string Message)> DeclineFriendRequestAsync(int friendshipId, string userId);

        Task<(bool Success, string Message)> DeleteFriendAsync(int friendshipId, string userId);
    }
}