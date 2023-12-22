using Core.Application.Specifications.Repositories;
using Core.Domain.Entities;
using Core.Domain.Repositories;
using Core.Domain.Utilities;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Specifications;

namespace Test.Infrastructure.Repositories;

[TestFixture]
public class RepositorySpecificationTest : BaseSetUpTest
{
    private IRepository<Account> accountRepository;

    [SetUp]
    public void Setup()
    {
        accountRepository = new Repository<Account>(context, new RepositorySpecificationEvaluator<Account>());
    }

    [Test]
    [Category("GetAccount")]
    public async Task Get_account_must_return_valid_data()
    {
        var data = await accountRepository
            .GetAsync(expression: _ => true, takeAll: true)
            .ContinueWith(t => t.Result.Values);
        Assert.That(data.Count > 0);
    }

    [Test]
    [Category("RepositorySpecification")]
    public async Task Get_account_must_return_valid_data_when_given_valid_input()
    {
        var id = Guid.Parse("b5e5a3c0-e9e9-4528-a362-4ca4aaf43bf0");
        var specification = new AccountSearchSpec(id);
        var data = await accountRepository.GetAsync(specification);
        Assert.Multiple(() =>
        {
            Assert.NotNull(data);
            Assert.NotNull(data.Values);
            Assert.IsNotEmpty(data.Values);
            Assert.That(data.Values[0].Id, Is.EqualTo(id));
        });
    }

    [Test]
    [Category("RepositorySpecification")]
    public async Task Get_account_must_return_empty_when_given_invalid_date()
    {
        var specification = new AccountSearchSpec(
            from: DateTimeHelper.VNDateTime.AddDays(10),
            to: DateTimeHelper.VNDateTime
        );
        var data = await accountRepository.GetAsync(specification);
        Assert.Multiple(() =>
        {
            Assert.NotNull(data);
            Assert.IsEmpty(data.Values);
        });
    }

    [Test]
    [Category("RepositorySpecification")]
    public async Task Get_account_must_return_valid_data_when_given_valid_from_to()
    {
        var spec = new AccountSearchSpec(
            from: DateTimeHelper.VNDateTime.AddDays(-10),
            to: DateTimeHelper.VNDateTime,
            pageSize: 10
        );
        var data = await accountRepository.GetAsync(spec);
        Assert.Multiple(() =>
        {
            Assert.NotNull(data);
            Assert.NotNull(data.Values);
            Assert.IsNotEmpty(data.Values);
        });
    }

    [Test]
    [Category("RepositorySpecification")]
    public async Task Get_account_must_return_empty_when_give_invalid_id()
    {
        var id = Guid.Parse("e9f8f27d-444d-41a7-b918-921b4afd5a63");
        var spec = new AccountSearchSpec(guid: id);
        var data = await accountRepository.GetAsync(spec);
        Assert.Multiple(() =>
        {
            Assert.NotNull(data);
            Assert.NotNull(data.Values);
            Assert.IsEmpty(data.Values, $"Expected empty but has found {data.Values.Count} records");
        });
    }

    [Test]
    [Category("RepositorySpecification")]
    public async Task Get_account_using_AccountByIdRepoSpecification_must_return_valid_data_when_given_valid_input()
    {
        var id = Guid.Parse("b5e5a3c0-e9e9-4528-a362-4ca4aaf43bf0");
        var spec = new AccountByIdRepoSpec(id);
        var data = await accountRepository.GetAsync(spec);
        Assert.Multiple(() =>
        {
            Assert.NotNull(data);
            Assert.IsNotEmpty(data.Values);
            Assert.NotNull(data.Values[0]);
            Assert.That(data.Values[0].Id, Is.EqualTo(id));
        });
    }

    [Test]
    [Category("RepositorySpecification")]
    public async Task Get_account_must_return_expected_quantity()
    {
        var pageSize = 3;
        var spec = new AccountSearchSpec(pageSize: pageSize);
        var data = await accountRepository.GetAsync(spec);
        Assert.Multiple(() =>
        {
            Assert.NotNull(data);
            Assert.IsNotEmpty(data.Values);
            Assert.That(data.Values.Count == pageSize);
        });
    }

    [Test]
    [Category("RepositorySpecification")]
    public async Task Get_account_using_AccountByIdRepoSpecification_must_return_empty_when_given_invalid_input()
    {
        var id = Guid.Parse("e9f8f27d-444d-41a7-b918-921b4afd5a63");
        var spec = new AccountByIdRepoSpec(id);
        var data = await accountRepository.GetAsync(spec);
        Assert.Multiple(() =>
        {
            Assert.NotNull(data);
            Assert.IsEmpty(data.Values);
        });
    }
}
