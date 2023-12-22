using Core.Domain.Entities;

namespace Test.Infrastructure.Repositories;

[TestFixture]
public class UnitOfWorkTest : BaseSetUpTest
{
    [Test]
    public async Task Add_new_shop_must_return_true()
    {
        var id = Guid.NewGuid();
        var shop = new Shop
        {
            Id = id,
            AddressLine = "test",
            Name = "Test",
            Phone = "test",
            WardId = Guid.Parse("cd147fbd-a6e7-4ae4-b0ac-119651b710c9")
        };
        await unitOfWork.Shops.AddAsync(shop);
        await unitOfWork.CompleteAsync();
        var find = await unitOfWork.Shops.GetByIdAsync(id);
        Assert.Multiple(() =>
        {
            Assert.NotNull(find);
            if (find != null)
                Assert.That(find.Id == id);
        });
    }

    [Test]
    public async Task Update_shop_must_return_true()
    {
        var name = "new name";
        var find = await unitOfWork.Shops.GetByIdAsync(Guid.Parse("cd147fbd-a6e7-4ae4-b0ac-119651b710c9"));
        Assert.NotNull(find);
        if (find != null)
        {
            find.Name = name;
            await unitOfWork.CompleteAsync();
            find = await unitOfWork.Shops.GetByIdAsync(Guid.Parse("cd147fbd-a6e7-4ae4-b0ac-119651b710c9"));
            Assert.That(find.Name == name);
        }
    }
}
