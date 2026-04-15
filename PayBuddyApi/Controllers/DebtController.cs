using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayBuddyApi.DTO.Debt;
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

        [HttpPost("CreateDebt")]
        public async Task<IActionResult> CreateDebt(DebtForSaveDTO dto)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var created = await _debtService.CreateDebtAsync(userId, dto);
            if (!created)
                return BadRequest("Could not create debt.");

            return Ok();
        }

        [HttpPut("MarkAsPaid/{id}")]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var updated = await _debtService.MarkAsPaidAsync(id, userId);
            if (!updated)
                return BadRequest("Could not update debt.");

            return Ok();
        }
    }
}