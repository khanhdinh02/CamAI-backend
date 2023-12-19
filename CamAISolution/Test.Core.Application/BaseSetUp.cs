using Core.Application;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Moq;

namespace Test.Core.Application;

[SetUpFixture]
public class BaseSetUp
{
    protected IUnitOfWork unitOfWork;
    protected IAppLogging<ShopService> logging;

    [OneTimeSetUp]
    public void SetUp()
    {
        logging = new Mock<IAppLogging<ShopService>>().Object;
        var mockUOW = new Mock<IUnitOfWork>();
        mockUOW.Setup(uow => uow.Shops.AddAsync(It.IsAny<Shop>())).ReturnsAsync(new Shop() { });
        unitOfWork = mockUOW.Object;
    }
}
