using Application.Common;
using Domain.Models;
using Infrastructure.Auth.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Exceptions.Auth;
using Shared.Messages.Identity;
using Shared.Wrapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Identity;

public class TokenService : ITokenService
{
	private readonly UserManager<AppUser> _userManager;
	private readonly JwtSettings _jwtSettings;

	public TokenService(UserManager<AppUser> userManager, IOptions<JwtSettings> jwtSettings)
	{
		_userManager = userManager;
		_jwtSettings = jwtSettings.Value;
	}
	public async Task<IResult<IDR_002>> GetTokenAsync(IDM_002 request, string ipAddress, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByEmailAsync(request.Email.Trim().Normalize());
		if (user is null)
		{
			throw new UnauthorizedException("Пользователь не найден");
		}

		if (!user.IsActive)
		{
			throw new UnauthorizedException("Пользователь не активен");
		}

		if (!await _userManager.CheckPasswordAsync(user, request.Password))
		{
			throw new UnauthorizedException("Неправильный e-mail или пароль");
		}

		return await GenerateTokensAndUpdateUser(user, ipAddress);
	}

	private async Task<IResult<IDR_002>> GenerateTokensAndUpdateUser(AppUser user, string ipAddress)
	{
		string token = await GenerateJwt(user, ipAddress);

		user.RefreshToken = GenerateRefreshToken();
		user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.TokenExpirationInDays);

		await _userManager.UpdateAsync(user);
		var response = new IDR_002() { Token = token, RefreshToken = user.RefreshToken, RefreshTokenExpiryTime = user.RefreshTokenExpiryTime };
		return await Result<IDR_002>.SuccessAsync(response);
	}

	private async Task<string> GenerateJwt(AppUser user, string ipAddress) =>
		GenerateEncryptedToken(GetSigningCredentials(), await GetClaims(user, ipAddress));

	private async Task<IEnumerable<Claim>> GetClaims(AppUser user, string ipAddress)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, user.Id),
			new(ClaimTypes.Email, user.Email),
			new(ClaimTypes.Name, user.FullName),
			new("ipAddress", ipAddress),
			new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
		};

		var roles = await _userManager.GetRolesAsync(user);
		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}
		return claims;
	}
		

	private string GenerateRefreshToken()
	{
		byte[] randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
	{
		var token = new JwtSecurityToken(
			claims: claims,
			expires: DateTime.UtcNow.AddDays(_jwtSettings.TokenExpirationInDays),
			signingCredentials: signingCredentials);
		var tokenHandler = new JwtSecurityTokenHandler();
		return tokenHandler.WriteToken(token);
	}
	private SigningCredentials GetSigningCredentials()
	{
		if (string.IsNullOrEmpty(_jwtSettings.Key))
		{
			throw new InvalidOperationException("No Key defined in JwtSettings config.");
		}

		byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
		return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
	}
}
