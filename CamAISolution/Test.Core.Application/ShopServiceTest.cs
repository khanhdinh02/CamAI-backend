using Core.Application;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Core.Domain.Models.DTOs.Shops;
using Moq;

namespace Test.Core.Application;

[TestFixture]
public class ShopServiceTest : BaseSetUp
{
    private ShopService shopService;

    [SetUp]
    public void ShopServiceTestSetUp()
    {
        shopService = new ShopService(unitOfWork, logging);
    }


    [Test]
    public async Task Return_true_when_given_valid_input()
    {
        var data = await shopService.GetShops(new SearchShopRequest()
        {
            Name = "Test",
            StatusId = AppConstant.ShopActiveStatus,
            Size = 10,
            PageIndex = 0,
        });
        Assert.NotNull(data);
    }

    [Test]
    public void When_given_invalid_shop_id_must_throw_not_found_excpetion()
    {
        Assert.ThrowsAsync<NotFoundException>(async () => await shopService.UpdateShop(Guid.Empty, new UpdateShopDto()));
    }

    [Test]
    public void When_updating_inactive_shop_must_throw_bad_request_excpetion()
    {
        var mockUOW = new Mock<IUnitOfWork>();
        mockUOW.Setup(uow => uow.Shops.GetByIdAsync(It.Is<Guid>(id => id == Guid.Parse("0a984765-57df-4fb1-a9b8-304e3dd3b69c")))).ReturnsAsync(new Shop
        {
            ShopStatusId = AppConstant.ShopInactiveStatus,
            Name = "Test"
        });
        shopService = new ShopService(mockUOW.Object, logging);
        Assert.ThrowsAsync<BadRequestException>(async () => await shopService.UpdateShop(Guid.Parse("0a984765-57df-4fb1-a9b8-304e3dd3b69c"), new UpdateShopDto()));
    }
}
