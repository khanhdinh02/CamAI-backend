using Core.Domain.Interfaces.Services;
using Grace.DependencyInjection.Attributes;

namespace Core.Application.Implements;

[ExportByInterfaces]
public class AccountService : IAccountService { }
