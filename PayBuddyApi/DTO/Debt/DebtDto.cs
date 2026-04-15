namespace PayBuddyApi.DTO.Debt
{
    public class DebtDto
    {
        public int DebtId { get; set; }
        public string CreditorName { get; set; }
        public string DebtorName { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
