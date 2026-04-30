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
        public string Status { get; set; } = string.Empty;
    }

    public class DebtDto : DebtForUpdateDTO
    {
        public required string CreditorName { get; set; }
        public required string DebtorName { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool CurrentUserIsCreditor { get; set; }
        public string DisplayText { get; set; } = string.Empty;

        public bool IsPaid => Status == "Paid";
        public bool IsPending => Status == "Pending";
        public bool IsAccepted => Status == "Accepted";
        public bool IsDeclined => Status == "Declined";
    }

    public class DebtRequestDto
    {
        public int DebtId { get; set; }

        public required string CreditorName { get; set; }
        public required string CreditorId { get; set; }

        public decimal Amount { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class DebtDTOMinusRelations : DebtForUpdateDTO
    {
        public DateTime CreatedAt { get; set; }
    }
}