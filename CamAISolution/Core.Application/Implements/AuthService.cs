using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories.Base;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.dtos.auth;
using Core.Domain.Models.enums;
using Core.Domain.Utilities;

namespace Core.Application.Implements;
public class AuthService( IJwtService jwtService) : IAuthService
{
    public async Task<TokenResponseDTO> getTokensByUsernameAndPassword(string username, string password)
    {
        /*Account account = await this.GetAccountByUsernameAndPassword(username, password);*/
        Account account = new Account() { Id= new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"), Username = username, Password= password, Role= "test" };
        string accessToken = jwtService.GenerateToken(account, TokenType.ACCESS_TOKEN);
        string refreshToken = jwtService.GenerateToken(account, TokenType.REFRESH_TOKEN);
        TokenResponseDTO tokenResponseDTO = new TokenResponseDTO() { AccessToken = accessToken, RefreshToken = refreshToken };
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

    public Guid test()
    {
       return jwtService.GetCurrentUserId();
    }

}
