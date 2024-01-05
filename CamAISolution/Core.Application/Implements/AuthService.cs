using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.DTO;
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
                    && (a.AccountStatusId == AccountStatusEnum.Active || a.AccountStatusId == AccountStatusEnum.New),
                orderBy: e => e.OrderBy(a => a.Id),
                includeProperties: [ nameof(Account.Roles), nameof(Account.AccountStatus) ]
            );
        if (foundAccount.Values.Count == 0)
            throw new UnauthorizedException("Wrong email or password");

        var account = foundAccount.Values[0];
        var isHashedCorrect = Hasher.Verify(password, account.Password);
        if (!isHashedCorrect)
            throw new UnauthorizedException("Wrong email or password");

        var accessToken = jwtService.GenerateToken(
            account.Id,
            account.Roles,
            account.AccountStatus,
            TokenType.AccessToken
        );
        var refreshToken = jwtService.GenerateToken(account.Id, account.Roles, TokenType.RefreshToken);
        return new TokenResponseDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    // TODO [Huy]: check account status before renew token
    // TODO [Huy]: check refreshToken in storage (redis)
    public string RenewToken(string oldAccessToken, string refreshToken)
    {
        var accessTokenDetail = jwtService.ValidateToken(oldAccessToken, TokenType.AccessToken, isValidateTime: false);
        var refreshTokenDetail = jwtService.ValidateToken(refreshToken, TokenType.RefreshToken);

        if (accessTokenDetail.UserId != refreshTokenDetail.UserId)
            throw new UnauthorizedException("Invalid Tokens");
        if (accessTokenDetail.Token == null)
            throw new UnauthorizedException("Invalid Tokens");

        return accessTokenDetail.Token;
    }

    public async Task ChangePassword(ChangePasswordDto changePasswordDto)
    {
        if (changePasswordDto.NewPassword != changePasswordDto.NewPasswordRetype)
            throw new BadRequestException("New password and retype password is not the same");

        if (changePasswordDto.OldPassword == changePasswordDto.NewPassword)
            throw new BadRequestException("New password cannot be the same as old password");

        var currentAccount = await accountService.GetCurrentAccount();
        if (!Hasher.Verify(changePasswordDto.OldPassword, currentAccount.Password))
            throw new UnauthorizedException("Wrong password");

        currentAccount.Password = Hasher.Hash(changePasswordDto.NewPassword);
        currentAccount.AccountStatusId = AccountStatusEnum.Active;
        unitOfWork.Accounts.Update(currentAccount);
        unitOfWork.Complete();
    }
}
