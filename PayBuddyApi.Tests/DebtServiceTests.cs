using PayBuddyApi.DTO.Debt;
using PayBuddyApi.Models;
using PayBuddyApi.Services;
using PayBuddyApi.Tests.TestHelpers;
using Xunit;

namespace PayBuddyApi.Tests;

public class DebtServiceTests
{
    [Fact]
    public async Task CreateDebtAsync_Should_Create_Pending_Debt_When_Users_Are_Friends()
    {
        var (context, userManager) = await TestDbFactory.CreateAsync();

        var creditor = new AppUser { UserName = "creditor" };
        var debtor = new AppUser { UserName = "debtor" };

        await userManager.CreateAsync(creditor, "Password123!");
        await userManager.CreateAsync(debtor, "Password123!");

        context.Friendships.Add(new Friendship
        {
            UserId = creditor.Id,
            FriendId = debtor.Id,
            Status = FriendshipStatus.Accepted,
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        var service = new DebtService(context);

        var dto = new DebtForSaveDTO
        {
            DebtorId = debtor.Id,
            Amount = 100,
            Description = "Pizza"
        };

        var result = await service.CreateDebtAsync(creditor.Id, dto);

        Assert.True(result);

        var debt = context.Debts.FirstOrDefault();

        Assert.NotNull(debt);
        Assert.Equal(creditor.Id, debt.CreditorId);
        Assert.Equal(debtor.Id, debt.DebtorId);
        Assert.Equal(100, debt.Amount);
        Assert.Equal(DebtStatus.Pending, debt.Status);
    }

    [Fact]
    public async Task MarkAsPaidAsync_Should_Only_Work_For_Creditor()
    {
        var (context, userManager) = await TestDbFactory.CreateAsync();

        var creditor = new AppUser { UserName = "creditor" };
        var debtor = new AppUser { UserName = "debtor" };

        await userManager.CreateAsync(creditor, "Password123!");
        await userManager.CreateAsync(debtor, "Password123!");

        var debt = new Debt
        {
            CreditorId = creditor.Id,
            DebtorId = debtor.Id,
            Amount = 100,
            Description = "Pizza",
            Status = DebtStatus.Accepted,
            CreatedAt = DateTime.UtcNow
        };

        context.Debts.Add(debt);
        await context.SaveChangesAsync();

        var service = new DebtService(context);

        var debtorResult = await service.MarkAsPaidAsync(debt.DebtId, debtor.Id);
        var creditorResult = await service.MarkAsPaidAsync(debt.DebtId, creditor.Id);

        Assert.False(debtorResult);
        Assert.True(creditorResult);

        var updatedDebt = context.Debts.First(d => d.DebtId == debt.DebtId);
        Assert.Equal(DebtStatus.Paid, updatedDebt.Status);
    }
}