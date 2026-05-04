using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PayBuddyApi.Contexts;
using PayBuddyApi.Models;

namespace PayBuddyApi.Tests.TestHelpers;

public static class TestDbFactory
{
    public static async Task<(PayBuddyDbContext Context, UserManager<AppUser> UserManager)> CreateAsync()
    {
        var services = new ServiceCollection();

        services.AddLogging();

        services.AddDbContext<PayBuddyDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        services
            .AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<PayBuddyDbContext>()
            .AddDefaultTokenProviders();

        var provider = services.BuildServiceProvider();

        var context = provider.GetRequiredService<PayBuddyDbContext>();
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();

        await context.Database.EnsureCreatedAsync();

        return (context, userManager);
    }
}