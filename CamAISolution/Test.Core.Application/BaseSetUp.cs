using System.Linq.Expressions;
using Core.Application;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Core.Domain.Models;
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
        mockUOW.Setup(uow => uow.Wards.GetAsync(It.IsAny<Expression<Func<Ward, bool>>>(), default, default, default, default, default, default)).ReturnsAsync(new PaginationResult<Ward>()
        {
            PageIndex = 0,
            PageSize = 1,
            TotalCount = 1,
            Values = new List<Ward>
            {
                new Ward { Id = Guid.Parse("0a984765-57df-4fb1-a9b8-304e3dd3b69c"), Name = "Test"}
            }
        });
        mockUOW.Setup(uow => uow.Shops.AddAsync(It.IsAny<Shop>())).ReturnsAsync(new Shop());
        mockUOW.Setup(uow => uow.Shops.GetByIdAsync(It.Is<Guid>(id => id == Guid.Empty))).Returns(Task.FromResult<Shop?>(null));
        mockUOW.Setup(uow => uow.Shops.GetByIdAsync(It.Is<Guid>(id => id != Guid.Empty))).ReturnsAsync(new Shop());
        unitOfWork = mockUOW.Object;
    }
}
