using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Contracts.Identity;
using Template.Application.Features.Account;
using Template.Application.Model.Account;
using Template.Identity.Entities;

namespace Template.Identity.Services
{
    public class TokenUtilsService : ITokenUtils
    {
        private readonly AppIdentityDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenUtilsService(AppIdentityDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }


        public async Task<AuthenticationResponse> GenerateAuthenticationResponseForUserAsync(string uid, JwtSettings jwtSettings)
        {
            var user = await _context.Users.SingleAsync(u => u.Id == uid);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = GenerateUserClaims(userClaims, roles, user.Id, user.UserName, user.Email);
            var jwtSecurityToken = GenerateJwtSecurityToken(claims, jwtSettings);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);


            var refreshToken = new RefreshToken
            {
                JwtId = jwtSecurityToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthenticationResponse
            {
                Token = token,
                RefreshToken = refreshToken.Token
            };
        }
        public IEnumerable<Claim> GenerateUserClaims(IList<Claim> userClaims, IList<string> userRoles, string uid, string userName, string userMail)
        {
            var roleClaims = new List<Claim>();

            for (int i = 0; i < userRoles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", userRoles[i]));
            }

            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userMail),
                new Claim("uid", uid)
            }
            .Union(userClaims)
            .Union(roleClaims);
        }
        public JwtSecurityToken GenerateJwtSecurityToken(IEnumerable<Claim> claims, JwtSettings jwtSettings)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(jwtSettings.TokenLifetime),
                signingCredentials: signingCredentials);
        }
        public ClaimsPrincipal GetPrincipalsFromToken(string token, TokenValidationParameters tokenValidationParameters)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }
        public bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                    jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture);
        }
        public bool JwtIsExpired(ClaimsPrincipal claimsPrincipal)
        {
            var expiryDateUnix = long.Parse(claimsPrincipal.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                                    .AddSeconds(expiryDateUnix);

            return expiryDateTimeUtc < DateTime.UtcNow;
        }
        public AuthenticationResponse ValidateDbRefreshToken(IRefreshToken storedRefreshToken, string jti)
        {
            var response = new AuthenticationResponse();
            if (storedRefreshToken == null)
            {
                response.ErrorMessage = "This refresh token does not exists.";
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                response.ErrorMessage = "This refresh token has expired.";

            }

            if (storedRefreshToken.Invalidated)
            {
                response.ErrorMessage = "This refresh token has been invalidated.";

            }
            if (storedRefreshToken.IsUsed)
            {
                response.ErrorMessage = "This refresh token has been used.";

            }
            if (storedRefreshToken.JwtId != jti)
            {
                response.ErrorMessage = "This  refresh token does not match this JWT.";
            }

            return response;
        }
    }
}
