using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Host.CamAI.API;
using Infrastructure.Mapping.Profiles;

namespace Test.Host.CamAI.API;

[TestFixture]
public class MapperTest
{
    private IMapper mapper;

    [OneTimeSetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ShopProfile>());
        mapper = config.CreateMapper();
    }

    [Test]
    public void Map_to_ShopDto_must_return_true_when_given_valid_ShopEntity()
    {
        Shop shop =
            new()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Name = "Test",
                ModifiedDate = DateTime.Now.AddDays(1)
            };
        var mappedShop = mapper.Map<ShopDto>(shop);
        Assert.Multiple(() =>
        {
            Assert.NotNull(mappedShop);
            Assert.IsNotEmpty(mappedShop.Name);
            Assert.That(
                string.Compare(mappedShop.Name, shop.Name, StringComparison.Ordinal),
                Is.EqualTo(0),
                $"Expected {shop.Name} but was {mappedShop.Name}"
            );
            Assert.That(mappedShop.CreatedDate, Is.EqualTo(shop.CreatedDate));
            Assert.That(mappedShop.Id, Is.EqualTo(shop.Id));
            Assert.That(mappedShop.ModifiedDate, Is.EqualTo(shop.ModifiedDate));
        });
    }
}
