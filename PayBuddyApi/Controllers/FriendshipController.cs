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

            return Ok(await _friendshipService.GetFriendsAsync(userId));
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            return Ok(await _friendshipService.GetFriendRequestsAsync(userId));
        }

        [HttpPost("request")]
        public async Task<IActionResult> SendRequest(FriendForSaveDTO dto)
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
        public async Task<IActionResult> Accept(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var success = await _friendshipService.AcceptFriendRequestAsync(id, userId);

            if (!success)
                return BadRequest();

            return Ok();
        }

        [HttpPut("decline/{id}")]
        public async Task<IActionResult> Decline(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var success = await _friendshipService.DeclineFriendRequestAsync(id, userId);

            if (!success)
                return BadRequest();

            return Ok();
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

            return Ok();
        }
    }
}