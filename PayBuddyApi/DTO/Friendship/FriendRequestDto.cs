namespace PayBuddyApi.DTO.Friendship
{
    public class FriendRequestDto
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}