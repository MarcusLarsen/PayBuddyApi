using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PayBuddyApi.Contexts;
using PayBuddyApi.DTO.Friendship;
using PayBuddyApi.Interfaces;
using PayBuddyApi.Models;

namespace PayBuddyApi.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly PayBuddyDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public FriendshipService(PayBuddyDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<FriendDto>> GetFriendsAsync(string userId)
        {
            return await _context.Friendships
                .Include(f => f.Friend)
                .Where(f => f.UserId == userId)
                .Select(f => new FriendDto
                {
                    Id = f.Id,
                    FriendId = f.FriendId,
                    FriendUserName = f.Friend.UserName
                })
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> AddFriendAsync(string userId, FriendForSaveDTO dto)
        {
            var friendUser = await _userManager.FindByNameAsync(dto.FriendUserName);

            if (friendUser == null)
                return (false, "User not found.");

            if (friendUser.Id == userId)
                return (false, "You cannot add yourself as a friend.");

            var alreadyFriends = await _context.Friendships.AnyAsync(f =>
            (f.UserId == userId && f.FriendId == friendUser.Id) ||
            (f.UserId == friendUser.Id && f.FriendId == userId));

            if (alreadyFriends)
                return (false, "You are already friends.");

            var friendship = new Friendship
            {
                UserId = userId,
                FriendId = friendUser.Id
            };

            var reverseFriendship = new Friendship
            {
                UserId = friendUser.Id,
                FriendId = userId
            };

            _context.Friendships.Add(friendship);
            _context.Friendships.Add(reverseFriendship);
            await _context.SaveChangesAsync();

            return (true, "Friend added successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteFriendAsync(int friendshipId, string userId)
        {
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => f.Id == friendshipId && f.UserId == userId);

            if (friendship == null)
                return (false, "Friendship not found.");

            var reverseFriendship = await _context.Friendships
                .FirstOrDefaultAsync(f =>
                f.UserId == friendship.FriendId &&
                f.FriendId == friendship.UserId);

            _context.Friendships.Remove(friendship);

            if (reverseFriendship != null)
                _context.Friendships.Remove(reverseFriendship);

            await _context.SaveChangesAsync();
            return (true, "Friend removed successfully.");
        }
    }
}
