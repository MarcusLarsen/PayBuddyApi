using Microsoft.EntityFrameworkCore;
using PayBuddyApi.Contexts;
using PayBuddyApi.DTO.Debt;
using PayBuddyApi.Interfaces;
using PayBuddyApi.Models;

namespace PayBuddyApi.Services
{
    public class DebtService : IDebtService
    {
        private readonly PayBuddyDbContext _context;

        public DebtService(PayBuddyDbContext context)
        {
            _context = context;
        }

        public async Task<List<DebtDto>> GetUserDebtsAsync(string userId)
        {
            return await _context.Debts
                .Include(d => d.Creditor)
                .Include(d => d.Debtor)
                .Where(d =>
                    (d.CreditorId == userId || d.DebtorId == userId) &&
                    d.Status != DebtStatus.Pending &&
                    d.Status != DebtStatus.Declined)
                .Select(d => new DebtDto
                {
                    DebtId = d.DebtId,
                    DebtorId = d.DebtorId,
                    Amount = d.Amount,
                    Description = d.Description,
                    Status = d.Status.ToString(),
                    CreditorName = d.Creditor!.UserName!,
                    DebtorName = d.Debtor!.UserName!,
                    CreatedAt = d.CreatedAt,

                    CurrentUserIsCreditor = d.CreditorId == userId,

                    DisplayText = d.CreditorId == userId
                        ? $"{d.Debtor!.UserName} skylder dig"
                        : $"Du skylder {d.Creditor!.UserName}"
                })
                .ToListAsync();
        }

        public async Task<List<DebtRequestDto>> GetDebtRequestsAsync(string userId)
        {
            return await _context.Debts
                .Include(d => d.Creditor)
                .Where(d => d.DebtorId == userId && d.Status == DebtStatus.Pending)
                .Select(d => new DebtRequestDto
                {
                    DebtId = d.DebtId,
                    CreditorId = d.CreditorId,
                    CreditorName = d.Creditor!.UserName!,
                    Amount = d.Amount,
                    Description = d.Description,
                    CreatedAt = d.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> CreateDebtAsync(string creditorId, DebtForSaveDTO dto)
        {
            if (dto.Amount <= 0)
                return false;

            var debtorExists = await _context.Users.AnyAsync(u => u.Id == dto.DebtorId);

            if (!debtorExists)
                return false;

            var areFriends = await _context.Friendships.AnyAsync(f =>
                f.UserId == creditorId &&
                f.FriendId == dto.DebtorId &&
                f.Status == FriendshipStatus.Accepted);

            if (!areFriends)
                return false;

            var debt = new Debt
            {
                CreditorId = creditorId,
                DebtorId = dto.DebtorId,
                Amount = dto.Amount,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                Status = DebtStatus.Pending
            };

            _context.Debts.Add(debt);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AcceptDebtAsync(int debtId, string userId)
        {
            var debt = await _context.Debts.FirstOrDefaultAsync(d =>
                d.DebtId == debtId &&
                d.DebtorId == userId &&
                d.Status == DebtStatus.Pending);

            if (debt == null)
                return false;

            debt.Status = DebtStatus.Accepted;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeclineDebtAsync(int debtId, string userId)
        {
            var debt = await _context.Debts.FirstOrDefaultAsync(d =>
                d.DebtId == debtId &&
                d.DebtorId == userId &&
                d.Status == DebtStatus.Pending);

            if (debt == null)
                return false;

            debt.Status = DebtStatus.Declined;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MarkAsPaidAsync(int debtId, string userId)
        {
            var debt = await _context.Debts.FirstOrDefaultAsync(d =>
                d.DebtId == debtId &&
                d.Status == DebtStatus.Accepted);

            if (debt == null)
                return false;

            if (debt.CreditorId != userId)
                return false;

            debt.Status = DebtStatus.Paid;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}