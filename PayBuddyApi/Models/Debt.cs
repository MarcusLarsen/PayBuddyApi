using System.ComponentModel.DataAnnotations.Schema;

namespace PayBuddyApi.Models
{
    public class Debt
    {
        public int DebtId { get; set; }

        public string CreditorId { get; set; } 
        public string DebtorId { get; set; } 

        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsPaid { get; set; }

        [ForeignKey(nameof(CreditorId))]
        public AppUser Creditor { get; set; }

        [ForeignKey(nameof(DebtorId))]
        public AppUser Debtor { get; set; }
    }
}
