using PayBuddyApi.DTO.Friendship;
using PayBuddyApi.Models;
using PayBuddyApi.Services;
using PayBuddyApi.Tests.TestHelpers;
using Xunit;

namespace PayBuddyApi.Tests;
public class FriendshipServiceTests
{
    [Fact]
    public async Task SendFriendRequestAsync_Should_Create_Pending_Request()
    {
        var (context, userManager) = await TestDbFactory.CreateAsync();

        var userA = new AppUser { UserName = "userA" };
        var userB = new AppUser { UserName = "userB" };

        await userManager.CreateAsync(userA, "Password123!");
        await userManager.CreateAsync(userB, "Password123!");

        var service = new FriendshipService(context, userManager);

        var dto = new FriendForSaveDTO
        {
            FriendUserName = "userB"
        };

        var result = await service.SendFriendRequestAsync(userA.Id, dto);

        Assert.True(result.Success);

        var request = context.Friendships.FirstOrDefault();

        Assert.NotNull(request);
        Assert.Equal(userA.Id, request.UserId);
        Assert.Equal(userB.Id, request.FriendId);
        Assert.Equal(FriendshipStatus.Pending, request.Status);
    }

    [Fact]
    public async Task AcceptFriendRequestAsync_Should_Set_Request_To_Accepted_And_Create_Reverse_Friendship()
    {
        var (context, userManager) = await TestDbFactory.CreateAsync();

        var userA = new AppUser { UserName = "userA" };
        var userB = new AppUser { UserName = "userB" };

        await userManager.CreateAsync(userA, "Password123!");
        await userManager.CreateAsync(userB, "Password123!");

        var request = new Friendship
        {
            UserId = userA.Id,
            FriendId = userB.Id,
            Status = FriendshipStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        context.Friendships.Add(request);
        await context.SaveChangesAsync();

        var service = new FriendshipService(context, userManager);
        var result = await service.AcceptFriendRequestAsync(request.Id, userB.Id);
        Assert.True(result);

        var acceptedRequest = context.Friendships
            .First(f => f.Id == request.Id);

        Assert.Equal(FriendshipStatus.Accepted, acceptedRequest.Status);

        var reverseFriendship = context.Friendships
            .FirstOrDefault(f => f.UserId == userB.Id && f.FriendId == userA.Id);

        Assert.NotNull(reverseFriendship);
        Assert.Equal(FriendshipStatus.Accepted, reverseFriendship.Status);
    }
}