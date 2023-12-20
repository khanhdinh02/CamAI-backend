using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTOs.Auths;
using Core.Domain.Models.Enums;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class AuthService(IJwtService jwtService) : IAuthService
{
    public async Task<TokenResponseDto> GetTokensByUsernameAndPassword(string username, string password)
    {
        /*Account account = await this.GetAccountByUsernameAndPassword(username, password);*/
        var account = new Account
        {
            Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
            Username = username,
            Password = password,
            Role = ["test"]
        };
        string hash = Hasher.Hash("1234");
        bool isHashedCorrect = Hasher.Verify("123", hash);
        var accessToken = jwtService.GenerateToken(account.Id, account.Role, TokenType.AccessToken);
        var refreshToken = jwtService.GenerateToken(account.Id, account.Role, TokenType.RefreshToken);
        return new TokenResponseDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    //TODO: check account status - check refreshToken in storage
    public string RenewToken(string oldAccessToken, string refreshToken)
    {
        TokenDetailDto accessTokenDetail = jwtService.ValidateToken(oldAccessToken, TokenType.AccessToken);
        TokenDetailDto refreshTokenDetail = jwtService.ValidateToken(refreshToken, TokenType.RefreshToken);

        if (!accessTokenDetail.UserId.Equals(refreshTokenDetail.UserId))
        {
            throw new UnauthorizedException("Invalid Tokens");
        }

        return accessTokenDetail.Token;
    }

    /*private async Task<Account> GetAccountByUsernameAndPassword(string username, string password)
    {
        //TODO: CHECK USER STATUS
        //----: UPDATE/ADD ASSOSICATED TOKEN IN STORAGE
        var accounts = await accountRepo.GetAsync(expression: a => a.Username.Equals(username) && Hasher.Verify(password, a.Password));
        if (accounts.Values.Count <= 0)
            throw new NotFoundException(typeof(Account), username, this.GetType());

        return accounts.Values.First();
    }*/

    public Guid Test()
    {
        return jwtService.GetCurrentUserId();
    }
}
