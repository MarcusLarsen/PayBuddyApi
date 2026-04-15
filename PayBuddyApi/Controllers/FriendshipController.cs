using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayBuddyApi.DTO.Friendship;
using PayBuddyApi.Interfaces;
using System.Security.Claims;

namespace PayBuddyApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;

        public FriendshipController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        private string? GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }


        [HttpGet]
        public async Task<ActionResult<List<FriendDto>>> GetFriends()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var friends = await _friendshipService.GetFriendsAsync(userId);
            return Ok(friends);
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend(FriendForSaveDTO dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var result = await _friendshipService.AddFriendAsync(userId, dto);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFriend(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var result = await _friendshipService.DeleteFriendAsync(id, userId);
            
            if (!result.Success)
                return BadRequest(result.Message);
            
            return Ok(result.Message);
        }
    }
}