namespace PayBuddyApi.DTO.Debt
{
    public class DebtForSaveDTO
    {
        public required string DebtorId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

    public class DebtForUpdateDTO : DebtForSaveDTO
    {
        public int DebtId { get; set; }
        public bool IsPaid { get; set; }
    }

    public class DebtDto : DebtForUpdateDTO
    {
        public required string CreditorName { get; set; }
        public required string DebtorName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class DebtDTOMinusRelations : DebtForUpdateDTO
    {
        public DateTime CreatedAt { get; set; }
    }
}
