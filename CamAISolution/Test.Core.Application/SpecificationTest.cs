using Core.Application.Specifications.Accounts;
using Core.Domain.Utilities;
using Core.Domain.Entities;

namespace Test.Core.Application;

public class SpecificationTest
{
    [Test]
    [Category("AccountSpecification")]
    public void Account_specification_must_return_true_when_given_valid_input()
    {
        var guid = Guid.NewGuid();
        var account = new Account()
        {
            CreatedDate = DateTime.Now,
            Id = guid
        };

        var condition = new AccountByIdSpecification(guid).IsSatisfied(account);
        var combindeCondition = new AccountByIdSpecification(guid)
            .And(new AccountCreatedFromToSpecification(DateTimeHelper.VNDateTime.AddMinutes(-1), DateTimeHelper.VNDateTime.AddMinutes(1)))
            .IsSatisfied(account);
        Assert.Multiple(() =>
        {
            Assert.That(condition is true);
            Assert.That(combindeCondition is true);
        });
    }

    [Test]
    [Category("AccountSpecification")]
    public void Account_specification_must_return_false_when_given_invalid_id()
    {
        var account = new Account();
        var condition = new AccountByIdSpecification(Guid.NewGuid()).IsSatisfied(account);
        Assert.That(condition is false);
    }

    [Test]
    [Category("AccountSpecification")]
    public void Account_specification_must_return_false_when_given_invalid_from_to()
    {
        var account = new Account();
        var condition = new AccountCreatedFromToSpecification(DateTimeHelper.VNDateTime.AddDays(1), DateTimeHelper.VNDateTime).IsSatisfied(account);
        Assert.That(condition is false);
    }
}
