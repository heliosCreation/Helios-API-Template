using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Template.Application.Features.Account;
using Template.Application.Model.Account;
using Template.Application.Responses;

namespace Template.Application.Contracts.Identity
{
    public interface ITokenUtils
    {
        Task<AuthenticationResponse> GenerateAuthenticationResponseForUserAsync(string uid, JwtSettings jwtSettings);
        IEnumerable<Claim> GenerateUserClaims(IList<Claim> userClaims, IList<string> userRoles, string uid, string userName, string userMail);
        JwtSecurityToken GenerateJwtSecurityToken(IEnumerable<Claim> claims, JwtSettings jwtSettings);
        ClaimsPrincipal GetPrincipalsFromToken(string token, TokenValidationParameters tokenValidationParameters);
        bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken);
        bool JwtIsExpired(ClaimsPrincipal claimsPrincipal);
        ApiResponse<AuthenticationResponse> ValidateDbRefreshToken(IRefreshToken storedRefreshToken, string jti);
    }
}
