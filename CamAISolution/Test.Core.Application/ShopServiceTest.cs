using System.Linq.Expressions;
using Core.Application;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Core.Domain.Models;
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
    public void When_given_invalid_shop_id_must_throw_not_found_excpetion()
    {
        Assert.ThrowsAsync<NotFoundException>(async () => await shopService.UpdateShop(Guid.Empty, new UpdateShopDto()));
    }

    [Test]
    public void When_updating_inactive_shop_must_throw_bad_request_excpetion()
    {
        Assert.ThrowsAsync<BadRequestException>(async () => await shopService.UpdateShop(Guid.NewGuid(), new UpdateShopDto()));
    }
}
