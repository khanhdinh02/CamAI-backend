using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTO.Auths;
using Core.Domain.Models.Enums;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class AuthService(IJwtService jwtService) : IAuthService
{
    //TODO: ckeck user from storage
    public async Task<TokenResponseDto> GetTokensByUsernameAndPassword(string username, string password)
    {
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
}
