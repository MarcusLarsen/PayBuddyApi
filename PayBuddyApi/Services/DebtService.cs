using PayBuddyApi.Contexts;
using PayBuddyApi.DTO.Debt;
using PayBuddyApi.Interfaces;

namespace PayBuddyApi.Services
{
    public class DebtService : IDebtService
    {
        private readonly PayBuddyDbContext _context;

        public DebtService(PayBuddyDbContext Context)
        {
            _context = Context;
        }
    }
}
