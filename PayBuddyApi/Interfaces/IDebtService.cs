using PayBuddyApi.DTO.Debt;

namespace PayBuddyApi.Interfaces
{
    public interface IDebtService
    {
        Task<List<DebtDto>> GetUserDebtsAsync(string userId);
        Task<bool> CreateDebtAsync(string creditorId, DebtForSaveDTO dto);
        Task<bool> MarkAsPaidAsync(int debtId, string userId);
    }
}
