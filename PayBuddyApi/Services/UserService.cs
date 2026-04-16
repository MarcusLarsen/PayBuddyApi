using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PayBuddyApi.DTO.User;
using PayBuddyApi.Interfaces;
using PayBuddyApi.Models;

namespace PayBuddyApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserDto>> SearchUsersAsync(string searchTerm, string currentUserId)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<UserDto>();

            return await _userManager.Users
                .Where(u => u.Id != currentUserId && u.UserName != null && u.UserName.Contains(searchTerm))
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName
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
