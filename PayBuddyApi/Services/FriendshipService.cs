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
                .Where(f =>
                    f.UserId == userId &&
                    f.Status == FriendshipStatus.Accepted)
                .Select(f => new FriendDto
                {
                    Id = f.Id,
                    FriendId = f.FriendId,
                    FriendUserName = f.Friend.UserName
                })
                .ToListAsync();
        }

        public async Task<List<FriendRequestDto>> GetFriendRequestsAsync(string userId)
        {
            return await _context.Friendships
                .Include(f => f.User)
                .Where(f =>
                    f.FriendId == userId &&
                    f.Status == FriendshipStatus.Pending)
                .Select(f => new FriendRequestDto
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    UserName = f.User.UserName!,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> SendFriendRequestAsync(string userId, FriendForSaveDTO dto)
        {
            var friendUser = await _userManager.FindByNameAsync(dto.FriendUserName);

            if (friendUser == null)
                return (false, "User not found.");

            if (friendUser.Id == userId)
                return (false, "You cannot add yourself.");

            var exists = await _context.Friendships.AnyAsync(f =>
                (f.UserId == userId && f.FriendId == friendUser.Id) ||
                (f.UserId == friendUser.Id && f.FriendId == userId));

            if (exists)
                return (false, "Friend request already exists.");

            var request = new Friendship
            {
                UserId = userId,
                FriendId = friendUser.Id,
                Status = FriendshipStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Friendships.Add(request);
            await _context.SaveChangesAsync();

            return (true, "Friend request sent.");
        }

        public async Task<bool> AcceptFriendRequestAsync(int requestId, string userId)
        {
            var request = await _context.Friendships.FirstOrDefaultAsync(f =>
                f.Id == requestId &&
                f.FriendId == userId &&
                f.Status == FriendshipStatus.Pending);

            if (request == null)
                return false;

            request.Status = FriendshipStatus.Accepted;

            var reverse = new Friendship
            {
                UserId = userId,
                FriendId = request.UserId,
                Status = FriendshipStatus.Accepted,
                CreatedAt = DateTime.UtcNow
            };

            _context.Friendships.Add(reverse);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeclineFriendRequestAsync(int requestId, string userId)
        {
            var request = await _context.Friendships.FirstOrDefaultAsync(f =>
                f.Id == requestId &&
                f.FriendId == userId &&
                f.Status == FriendshipStatus.Pending);

            if (request == null)
                return false;

            request.Status = FriendshipStatus.Declined;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<(bool Success, string Message)> DeleteFriendAsync(int friendshipId, string userId)
        {
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => f.Id == friendshipId && f.UserId == userId);

            if (friendship == null)
                return (false, "Friendship not found.");

            var reverse = await _context.Friendships
                .FirstOrDefaultAsync(f =>
                    f.UserId == friendship.FriendId &&
                    f.FriendId == friendship.UserId);

            _context.Friendships.Remove(friendship);

            if (reverse != null)
                _context.Friendships.Remove(reverse);

            await _context.SaveChangesAsync();

            return (true, "Friend removed.");
        }
    }
}