using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTOs.Auths;
using Core.Domain.Models.Enums;

namespace Core.Application.Implements;

public class AuthService(IJwtService jwtService) : IAuthService
{
    public async Task<TokenResponseDTO> GetTokensByUsernameAndPassword(string username, string password)
    {
        /*Account account = await this.GetAccountByUsernameAndPassword(username, password);*/
        var account = new Account
        {
            Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
            Username = username,
            Password = password,
            Role = "test"
        };
        var accessToken = jwtService.GenerateToken(account, TokenType.AccessToken);
        var refreshToken = jwtService.GenerateToken(account, TokenType.RefreshToken);
        var tokenResponseDTO = new TokenResponseDTO { AccessToken = accessToken, RefreshToken = refreshToken };
        return tokenResponseDTO;
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
