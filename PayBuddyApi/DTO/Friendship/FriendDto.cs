namespace PayBuddyApi.DTO.Friendship
{
    public class FriendForSaveDTO
    {
        public required string FriendUserName { get; set; }
    }

    public class FriendForUpdateDTO : FriendForSaveDTO
    {
        public int Id { get; set; }
        public required string FriendId { get; set; }
    }

    public class FriendDto : FriendForUpdateDTO
    {

    }

    public class FriendDTOMinusRelations : FriendForUpdateDTO
    {

    }
    public class FriendRequestDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
