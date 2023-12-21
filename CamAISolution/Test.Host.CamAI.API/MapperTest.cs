using AutoMapper;
using Core.Domain;
using Core.Domain.Entities;
using Host.CamAI.API.Mappings;

namespace Test.Host.CamAI.API;

[TestFixture]
public class MapperTest
{
    private IMapper mapper;

    [OneTimeSetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CamAIProfile>());
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
            Assert.That(mappedShop.Name.CompareTo(shop.Name) == 0, $"Expected {shop.Name} but was {mappedShop.Name}");
            Assert.That(mappedShop.CreatedDate == shop.CreatedDate);
            Assert.That(mappedShop.Id == shop.Id);
            Assert.That(mappedShop.ModifiedDate == shop.ModifiedDate);
        });
    }
}
