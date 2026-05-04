using System.ComponentModel.DataAnnotations.Schema;

namespace PayBuddyApi.Models
{
    public class Friendship
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public string FriendId { get; set; }

        public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; }

        [ForeignKey(nameof(FriendId))]
        public AppUser Friend { get; set; }
    }
}