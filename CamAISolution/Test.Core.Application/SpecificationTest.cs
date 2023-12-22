using Core.Application.Specifications;
using Core.Domain.Entities;
using Core.Domain.Utilities;

namespace Test.Core.Application;

public class SpecificationTest
{
    [Test]
    [Category("AccountSpecification")]
    public void Account_specification_must_return_true_when_given_valid_input()
    {
        var guid = Guid.NewGuid();
        var account = new Account() { CreatedDate = DateTime.Now, Id = guid };

        var condition = new AccountByIdSpec(guid).IsSatisfied(account);
        var combineCondition = new AccountByIdSpec(guid)
            .And(
                new AccountCreatedFromToSpec(
                    DateTimeHelper.VNDateTime.AddMinutes(-1),
                    DateTimeHelper.VNDateTime.AddMinutes(1)
                )
            )
            .IsSatisfied(account);
        Assert.Multiple(() =>
        {
            Assert.That(condition);
            Assert.That(combineCondition);
        });
    }

    [Test]
    [Category("AccountSpecification")]
    public void Account_specification_must_return_false_when_given_invalid_id()
    {
        var account = new Account();
        var condition = new AccountByIdSpec(Guid.NewGuid()).IsSatisfied(account);
        Assert.That(condition is false);
    }

    [Test]
    [Category("AccountSpecification")]
    public void Account_specification_must_return_false_when_given_invalid_from_to()
    {
        var account = new Account();
        var condition = new AccountCreatedFromToSpec(
            DateTimeHelper.VNDateTime.AddDays(1),
            DateTimeHelper.VNDateTime
        ).IsSatisfied(account);
        Assert.That(condition is false);
    }
}
