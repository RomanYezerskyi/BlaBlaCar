using System.Security.Claims;
using IdentityServerJWT.API.Models;

namespace IdentityServerJWT.API.Interfaces
{
    public interface ITokenService
    {
        Task<AuthenticatedResponse> RefreshTokenAsync(TokenApiModel tokenApiModel);
        Task RevokeTokenAsync(ClaimsPrincipal principal);
        string GenerateAccessTokenTokenAsync(IEnumerable<Claim> claims);
        string GenerateRefreshTokenAsync();
        ClaimsPrincipal GetPrincipalFromExpiredTokenAsync(string token);
    }
}
