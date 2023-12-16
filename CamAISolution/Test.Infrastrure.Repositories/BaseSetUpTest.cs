using Core.Domain.Entities;
using Core.Domain.Utilities;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace Test.Infrastrure.Repositories;

[SetUpFixture]
public class BaseSetUpTest
{
    protected CamAIContext context;
    [OneTimeSetUp]
    public async Task BaseSetUp()
    {
        var options = new DbContextOptionsBuilder<CamAIContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        context = new CamAIContext(options);
        var listAccount = new List<Account>
        {
            new Account
            {
                Id = Guid.Parse("b5e5a3c0-e9e9-4528-a362-4ca4aaf43bf0"),
                CreatedDate = DateTimeHelper.VNDateTime.AddDays(-2)
            },
            new Account
            {
                Id = Guid.Parse("0a984765-57df-4fb1-a9b8-304e3dd3b69c"),
                CreatedDate = DateTimeHelper.VNDateTime.AddDays(-3)
            },
            new Account
            {
                Id = Guid.Parse("82c43639-81e4-4821-a037-029a3df0453f"),
                CreatedDate = DateTimeHelper.VNDateTime.AddDays(-1)
            },
            new Account
            {
                Id = Guid.Parse("cd147fbd-a6e7-4ae4-b0ac-119651b710c9"),
                CreatedDate = DateTimeHelper.VNDateTime.AddDays(-10)
            }
        };
        await context.Set<Account>().AddRangeAsync(listAccount);
        await context.SaveChangesAsync();
    }
}
