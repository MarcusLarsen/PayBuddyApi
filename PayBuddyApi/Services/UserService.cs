using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PayBuddyApi.Contexts;
using PayBuddyApi.DTO.User;
using PayBuddyApi.Interfaces;
using PayBuddyApi.Models;

namespace PayBuddyApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly PayBuddyDbContext _context;

        public UserService(UserManager<AppUser> userManager, PayBuddyDbContext Context)
        {
            _userManager = userManager;
            _context = Context;
        }

        public async Task<List<UserDto>> SearchUsersAsync(string searchTerm, string currentUserId)
        {
            var friendIds = await _context.Friendships
                .Where(f => f.UserId == currentUserId)
                .Select(f => f.FriendId)
                .ToListAsync();

            var query = _userManager.Users
                .Where(u => u.Id != currentUserId &&
                            u.UserName != null &&
                            !friendIds.Contains(u.Id));

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u => u.UserName!.Contains(searchTerm));
            }

            return await query
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName!
                })
                .ToListAsync();
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName!
                })
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
