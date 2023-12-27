using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTO.Accounts;
using Core.Domain.Models.DTO.Auths;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class AuthService(IJwtService jwtService, IRepository<Account> accountRepo) : IAuthService
{
    //TODO: ckeck user from storage
    public async Task<TokenResponseDto> GetTokensByUsernameAndPassword(string email, string password)
    {
        var foundAccount = await accountRepo.GetAsync(expression: a => a.Email == email && a.Password == password);
        if (foundAccount.Values.Count == 0)
            throw new UnauthorizedException("Wrong email or password");
        var account = foundAccount.Values[0];
        // string hash = Hasher.Hash("1234");
        // bool isHashedCorrect = Hasher.Verify("123", hash);
        var accessToken = jwtService.GenerateToken(account.Id, account.Roles, TokenTypeEnum.AccessToken);
        var refreshToken = jwtService.GenerateToken(account.Id, account.Roles, TokenTypeEnum.RefreshToken);
        return new TokenResponseDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    //TODO: check account status - check refreshToken in storage
    public string RenewToken(string oldAccessToken, string refreshToken)
    {
        TokenDetailDto accessTokenDetail = jwtService.ValidateToken(oldAccessToken, TokenTypeEnum.AccessToken);
        TokenDetailDto refreshTokenDetail = jwtService.ValidateToken(refreshToken, TokenTypeEnum.RefreshToken);

        if (accessTokenDetail.UserId != refreshTokenDetail.UserId)
        {
            throw new UnauthorizedException("Invalid Tokens");
        }

        if (accessTokenDetail.Token == null)
        {
            throw new UnauthorizedException("Invalid Tokens");
        }
        return accessTokenDetail.Token;
    }
}
