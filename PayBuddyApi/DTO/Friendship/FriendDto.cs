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
}
