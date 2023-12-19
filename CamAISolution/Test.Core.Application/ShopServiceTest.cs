using Core.Application;

namespace Test.Core.Application;

public class ShopServiceTest : BaseSetUp
{
    private ShopService shopService;

    [SetUp]
    public void ShopServiceTestSetUp()
    {
        shopService = new ShopService(unitOfWork, logging);
    }
}
