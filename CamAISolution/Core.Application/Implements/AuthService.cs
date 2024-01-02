using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTO.Accounts;
using Core.Domain.Models.DTO.Auths;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class AuthService(IJwtService jwtService, IRepository<Account> accountRepo) : IAuthService
{
    public async Task<TokenResponseDto> GetTokensByUsernameAndPassword(string email, string password)
    {
        var foundAccount = await accountRepo.GetAsync(
            expression: a => a.Email == email && a.AccountStatusId == AccountStatusEnum.Active,
            orderBy: e => e.OrderBy(a => a.Id),
            includeProperties: [ nameof(Account.Roles) ]
        );
        if (foundAccount.Values.Count == 0)
            throw new UnauthorizedException("Wrong email or password");

        var account = foundAccount.Values[0];
        var hashedPassword = Hasher.Hash(password);
        var isHashedCorrect = Hasher.Verify(account.Password, hashedPassword);
        if (!isHashedCorrect)
            throw new UnauthorizedException("Wrong email or password");

        var accessToken = jwtService.GenerateToken(account.Id, account.Roles, TokenTypeEnum.AccessToken);
        var refreshToken = jwtService.GenerateToken(account.Id, account.Roles, TokenTypeEnum.RefreshToken);
        return new TokenResponseDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    //TODO: check account status - check refreshToken in storage
    public string RenewToken(string oldAccessToken, string refreshToken)
    {
        var accessTokenDetail = jwtService.ValidateToken(oldAccessToken, TokenTypeEnum.AccessToken);
        var refreshTokenDetail = jwtService.ValidateToken(refreshToken, TokenTypeEnum.RefreshToken);

        if (accessTokenDetail.UserId != refreshTokenDetail.UserId)
            throw new UnauthorizedException("Invalid Tokens");
        if (accessTokenDetail.Token == null)
            throw new UnauthorizedException("Invalid Tokens");

        return accessTokenDetail.Token;
    }
}
