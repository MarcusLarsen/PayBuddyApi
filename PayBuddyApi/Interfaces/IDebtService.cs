using PayBuddyApi.DTO.Debt;

namespace PayBuddyApi.Interfaces
{
    public interface IDebtService
    {
        Task<List<DebtDto>> GetUserDebtsAsync(string userId);
        Task<List<DebtRequestDto>> GetDebtRequestsAsync(string userId);
        Task<bool> CreateDebtAsync(string creditorId, DebtForSaveDTO dto);
        Task<bool> AcceptDebtAsync(int debtId, string userId);
        Task<bool> DeclineDebtAsync(int debtId, string userId);
        Task<bool> MarkAsPaidAsync(int debtId, string userId);
    }
}