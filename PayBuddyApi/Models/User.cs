using Microsoft.AspNetCore.Identity;

namespace PayBuddyApi.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Friendship> Friends { get; set; }
    }
}
