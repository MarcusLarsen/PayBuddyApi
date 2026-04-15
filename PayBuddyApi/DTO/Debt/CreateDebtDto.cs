namespace PayBuddyApi.DTO.Debt
{
    public class CreateDebtDto
    {
        public required string DebtorId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
