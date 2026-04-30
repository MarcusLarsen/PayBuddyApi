using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBuddyApi.DTO.Debt;
using PayBuddyApi.DTO.Responses;
using PayBuddyApi.Interfaces;
using System.Security.Claims;

namespace PayBuddyApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DebtController : ControllerBase
    {
        private readonly IDebtService _debtService;

        public DebtController(IDebtService debtService)
        {
            _debtService = debtService;
        }

        private string? GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        [HttpGet("GetUserDebts")]
        public async Task<IActionResult> GetUserDebts()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var debts = await _debtService.GetUserDebtsAsync(userId);
            return Ok(debts);
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetDebtRequests()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var requests = await _debtService.GetDebtRequestsAsync(userId);
            return Ok(requests);
        }

        [HttpPost("CreateDebt")]
        public async Task<IActionResult> CreateDebt(DebtForSaveDTO dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var created = await _debtService.CreateDebtAsync(userId, dto);

            if (!created)
            {
                return BadRequest(new MessageResponseDto
                {
                    Message = "Could not create debt. User may not exist or is not your friend."
                });
            }

            return Ok(new MessageResponseDto
            {
                Message = "Debt request created."
            });
        }

        [HttpPut("accept/{id}")]
        public async Task<IActionResult> AcceptDebt(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var updated = await _debtService.AcceptDebtAsync(id, userId);

            if (!updated)
            {
                return BadRequest(new MessageResponseDto
                {
                    Message = "Could not accept debt."
                });
            }

            return Ok(new MessageResponseDto
            {
                Message = "Debt accepted."
            });
        }

        [HttpPut("decline/{id}")]
        public async Task<IActionResult> DeclineDebt(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var updated = await _debtService.DeclineDebtAsync(id, userId);

            if (!updated)
            {
                return BadRequest(new MessageResponseDto
                {
                    Message = "Could not decline debt."
                });
            }

            return Ok(new MessageResponseDto
            {
                Message = "Debt declined."
            });
        }

        [HttpPut("MarkAsPaid/{id}")]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var updated = await _debtService.MarkAsPaidAsync(id, userId);

            if (!updated)
            {
                return BadRequest(new MessageResponseDto
                {
                    Message = "Could not update debt."
                });
            }

            return Ok(new MessageResponseDto
            {
                Message = "Debt marked as paid."
            });
        }
    }
}