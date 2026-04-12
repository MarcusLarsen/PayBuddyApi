using System.ComponentModel.DataAnnotations.Schema;

namespace PayBuddyApi.Models
{
    public class Friendship
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public string FriendId { get; set; }

        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; }

        [ForeignKey(nameof(FriendId))]
        public AppUser Friend { get; set; }
    }
}
