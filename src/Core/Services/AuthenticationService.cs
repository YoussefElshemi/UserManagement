using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core.Configs;
using Core.Enums;
using Core.Helpers;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Core.Services;

public class AuthenticationService(
    IUserService userService,
    IPasswordResetService passwordResetService,
    IOneTimePasscodeService oneTimePasscodeService,
    ILoginService loginService,
    TimeProvider timeProvider,
    IOptions<AppConfig> config) : IAuthenticationService
{
    public async Task<AuthenticationResponse> CreateAsync(CreateUserRequest createUserRequest)
    {
        var user = await userService.CreateAsync(createUserRequest);
        var login = await loginService.CreateAsync(user, createUserRequest.IpAddress);

        return HandleAuthentication(login);
    }

    public async Task<AuthenticationResponse> AuthenticateAsync(User user, AuthenticationRequest authenticationRequest)
    {
        var login = await loginService.CreateAsync(user, authenticationRequest.IpAddress);

        return HandleAuthentication(login);
    }

    public async Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        var login = await loginService.GetAsync(refreshTokenRequest.RefreshToken);
        return HandleAuthentication(login);
    }

    public async Task<AuthenticationResponse> TwoFactorAuthenticationAsync(TwoFactorRequest twoFactorRequest)
    {
        var user = await userService.GetAsync(twoFactorRequest.Username);
        var oneTimePasscode = await oneTimePasscodeService.GetAsync(user.UserId, twoFactorRequest.Passcode);
        var login = await loginService.CreateAsync(user, twoFactorRequest.IpAddress);

        oneTimePasscode = oneTimePasscode with
        {
            IsUsed = new IsUsed(true),
            UpdatedBy = user.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        await oneTimePasscodeService.UpdateAsync(oneTimePasscode);

        return HandleAuthentication(login);
    }

    public async Task RequestTwoFactorAsync(User user, AuthenticationRequest authenticationRequest)
    {
        await oneTimePasscodeService.InitiateAsync(user, authenticationRequest.IpAddress);
    }

    public async Task RevokeToken(RefreshTokenRequest refreshTokenRequest)
    {
        var login = await loginService.GetAsync(refreshTokenRequest.RefreshToken);

        login = login with
        {
            IsRevoked = new IsRevoked(true),
            RevokedBy = login.User.Username,
            RevokedAt = new RevokedAt(timeProvider.GetUtcNow().UtcDateTime),
            UpdatedBy = login.User.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        await loginService.UpdateAsync(login);
    }

    public async Task RequestPasswordReset(RequestPasswordResetRequest requestPasswordResetRequest)
    {
        var userExists = await userService.ExistsAsync(requestPasswordResetRequest.EmailAddress);
        if (!userExists)
        {
            return;
        }

        var user = await userService.GetAsync(requestPasswordResetRequest.EmailAddress);
        await passwordResetService.InitiateAsync(user);
    }

    public async Task<User> ChangePasswordAsync(User user, Password password)
    {
        var passwordSalt = new PasswordSalt(Convert.ToBase64String(SecurityTokenHelper.GenerateSalt()));
        var passwordHash = HashPassword(password, passwordSalt);

        var updatedUser = user with
        {
            PasswordHash = new PasswordHash(passwordHash),
            PasswordSalt = passwordSalt,
            UpdatedBy = user.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        await userService.UpdateAsync(updatedUser);

        return updatedUser;
    }

    public async Task ResetPassword(ResetPasswordRequest resetPasswordRequest)
    {
        var passwordReset = await passwordResetService.GetAsync(resetPasswordRequest.PasswordResetToken);

        passwordReset = passwordReset with
        {
            IsUsed = new IsUsed(true),
            UpdatedBy = new Username(Username.SystemUsername),
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        await passwordResetService.UpdateAsync(passwordReset);

        await ChangePasswordAsync(passwordReset.User, resetPasswordRequest.Password);
    }

    public bool Authenticate(User user, Password password)
    {
        return user.PasswordHash == HashPassword(password, user.PasswordSalt);
    }

    private JwtSecurityToken CreateJwtSecurityToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.JwtConfig.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.UserRole.ToString())
        };

        return new JwtSecurityToken(
            config.Value.JwtConfig.Issuer,
            config.Value.JwtConfig.Audience,
            expires: timeProvider.GetUtcNow().UtcDateTime.AddMinutes(config.Value.JwtConfig.AccessTokenLifetimeInMinutes),
            notBefore: timeProvider.GetUtcNow().UtcDateTime,
            claims: claims,
            signingCredentials: credentials
        );
    }

    public static string HashPassword(Password password, PasswordSalt salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltBytes = Convert.FromBase64String(salt);
        var saltedPassword = new byte[passwordBytes.Length + saltBytes.Length];

        Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
        Buffer.BlockCopy(saltBytes, 0, saltedPassword, passwordBytes.Length, saltBytes.Length);

        var hashedBytes = SHA256.HashData(saltedPassword);

        var hashedPasswordWithSalt = new byte[hashedBytes.Length + saltBytes.Length];
        Buffer.BlockCopy(saltBytes, 0, hashedPasswordWithSalt, 0, saltBytes.Length);
        Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSalt, saltBytes.Length, hashedBytes.Length);

        return Convert.ToBase64String(hashedPasswordWithSalt);
    }

    private AuthenticationResponse HandleAuthentication(Login login)
    {
        var jwtSecurityToken = CreateJwtSecurityToken(login.User);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        var accessTokenExpiresIn = Convert.ToInt32((jwtSecurityToken.ValidTo - jwtSecurityToken.ValidFrom).TotalSeconds);

        var refreshTokenExpiresIn = Convert.ToInt32((login.ExpiresAt - timeProvider.GetUtcNow().UtcDateTime).TotalSeconds);

        return new AuthenticationResponse
        {
            UserId = login.User.UserId,
            TokenType = TokenType.Bearer,
            AccessToken = new AccessToken(accessToken),
            AccessTokenExpiresIn = new ExpiresIn(accessTokenExpiresIn),
            RefreshToken = login.RefreshToken,
            RefreshTokenExpiresIn = new ExpiresIn(refreshTokenExpiresIn)
        };
    }
}