using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBuddyApi.DTO.Friendship;
using PayBuddyApi.DTO.Responses;
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

        [HttpGet("requests")]
        public async Task<ActionResult<List<FriendRequestDto>>> GetFriendRequests()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var requests = await _friendshipService.GetFriendRequestsAsync(userId);
            return Ok(requests);
        }

        [HttpPost("request")]
        public async Task<IActionResult> SendFriendRequest(FriendForSaveDTO dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var result = await _friendshipService.SendFriendRequestAsync(userId, dto);

            if (!result.Success)
                return BadRequest(new MessageResponseDto { Message = result.Message });

            return Ok(new MessageResponseDto { Message = result.Message });
        }

        [HttpPut("accept/{id}")]
        public async Task<IActionResult> AcceptFriendRequest(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var result = await _friendshipService.AcceptFriendRequestAsync(id, userId);

            if (!result.Success)
                return BadRequest(new MessageResponseDto { Message = result.Message });

            return Ok(new MessageResponseDto { Message = result.Message });
        }

        [HttpPut("decline/{id}")]
        public async Task<IActionResult> DeclineFriendRequest(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var result = await _friendshipService.DeclineFriendRequestAsync(id, userId);

            if (!result.Success)
                return BadRequest(new MessageResponseDto { Message = result.Message });

            return Ok(new MessageResponseDto { Message = result.Message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFriend(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var result = await _friendshipService.DeleteFriendAsync(id, userId);

            if (!result.Success)
                return BadRequest(new MessageResponseDto { Message = result.Message });

            return Ok(new MessageResponseDto { Message = result.Message });
        }
    }
}