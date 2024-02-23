using Core.Application.Exceptions;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class AuthService(IJwtService jwtService, IAccountService accountService, IUnitOfWork unitOfWork) : IAuthService
{
    public async Task<TokenResponseDto> GetTokensByUsernameAndPassword(string email, string password)
    {
        var foundAccount = await unitOfWork
            .Accounts
            .GetAsync(
                expression: a =>
                    a.Email == email
                    && (a.AccountStatus == AccountStatus.Active || a.AccountStatus == AccountStatus.New),
                orderBy: e => e.OrderBy(a => a.Id),
                includeProperties: [nameof(Account.Roles)]
            );
        if (foundAccount.Values.Count == 0)
            throw new UnauthorizedException("Wrong email or password");

        var account = foundAccount.Values[0];
        var isHashedCorrect = Hasher.Verify(password, account.Password);
        if (!isHashedCorrect)
            throw new UnauthorizedException("Wrong email or password");

        var roles = account.Roles.Select(ar => ar.Role);
        var accessToken = jwtService.GenerateToken(account.Id, roles, account.AccountStatus, TokenType.AccessToken);
        var refreshToken = jwtService.GenerateToken(account.Id, roles, TokenType.RefreshToken);
        return new TokenResponseDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    // TODO [Huy]: check account status before renew token
    // TODO [Huy]: check refreshToken in storage (redis)
    public async Task<string> RenewToken(string oldAccessToken, string refreshToken)
    {
        var accessTokenDetail = jwtService.ValidateToken(oldAccessToken, TokenType.AccessToken, isValidateTime: false);
        var refreshTokenDetail = jwtService.ValidateToken(refreshToken, TokenType.RefreshToken);

        if (accessTokenDetail.UserId != refreshTokenDetail.UserId)
            throw new UnauthorizedException("Invalid Tokens");
        if (accessTokenDetail.Token == null)
            throw new UnauthorizedException("Invalid Tokens");

        var account = await accountService.GetAccountById(accessTokenDetail.UserId);
        return jwtService.GenerateToken(
            account.Id,
            account.Roles.Select(ar => ar.Role),
            account.AccountStatus,
            TokenType.AccessToken
        );
    }

    public async Task ChangePassword(ChangePasswordDto changePasswordDto)
    {
        if (changePasswordDto.NewPassword != changePasswordDto.NewPasswordRetype)
            throw new BadRequestException("New password and retype password is not the same");

        if (changePasswordDto.OldPassword == changePasswordDto.NewPassword)
            throw new BadRequestException("New password cannot be the same as old password");

        var currentAccount = accountService.GetCurrentAccount();
        if (!Hasher.Verify(changePasswordDto.OldPassword, currentAccount.Password))
            throw new UnauthorizedException("Wrong password");

        currentAccount.Password = Hasher.Hash(changePasswordDto.NewPassword);
        currentAccount.AccountStatus = AccountStatus.Active;
        unitOfWork.Accounts.Update(currentAccount);
        unitOfWork.Complete();
    }
}
