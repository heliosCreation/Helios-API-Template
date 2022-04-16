using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Contracts.Identity;
using Template.Application.Features.Account.Command;
using Template.Application.Model.Account;
using Template.Application.Model.Account.Authentification;
using Template.Application.Models.Account.RefreshToken;
using Template.Application.Responses;
using Template.Identity.Entities;

namespace Template.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppIdentityDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;


        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            AppIdentityDbContext context, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _tokenValidationParameters = tokenValidationParameters ?? throw new ArgumentNullException(nameof(tokenValidationParameters));
        }


        public async Task<ApiResponse<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var response = new ApiResponse<AuthenticationResponse>();
            var user = await _context.Users.Where(u => u.Email == request.Email).FirstOrDefaultAsync();
            if (user == null)
            {
                return response.SetUnhautorizedResponse();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return response.SetUnhautorizedResponse();
            }

            response.Data = await GenerateAuthenticationResponseForUserAsync(user);

            return response;
        }

        public async Task<ApiResponse<AuthenticationResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var response = new ApiResponse<AuthenticationResponse>();

            var claimPrincipals = GetPrincipalsFromToken(request.Token);
            if (claimPrincipals == null)
            {
                return response.SetBadRequestResponse(message: "Invalid Token");
            }

            var expiryDateUnix = long.Parse(claimPrincipals.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                                    .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return response.SetBadRequestResponse(message: "Token hasn't expired yet.");
            }

            var jti = claimPrincipals.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == request.RefreshToken);

            if (storedRefreshToken == null)
            {
                return response.setNotFoundResponse(message: "This refresh token does not exists.");
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return response.SetBadRequestResponse(message: "This refresh token has expired.");
            }

            if (storedRefreshToken.Invalidated)
            {
                return response.SetBadRequestResponse(message: "This refresh token has been invalidated.");
            }
            if (storedRefreshToken.IsUsed)
            {
                return response.SetBadRequestResponse(message: "This refresh token has been used.");
            }
            if (storedRefreshToken.JwtId != jti)
            {
                return response.SetBadRequestResponse(message: "This  refresh token does not match this JWT.");
            }

            storedRefreshToken.IsUsed = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(claimPrincipals.Claims.Single(c => c.Type == "uid").Value);
            response.Data =  await GenerateAuthenticationResponseForUserAsync(user);

            return response;
        }
        public async Task<CustomIdentityResult> RegisterAsync(RegisterUserCommand command)
        {
            var user = new ApplicationUser
            {
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                UserName = command.UserName,
                EmailConfirmed = false
            };
            var result = await _userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
            {
                return new CustomIdentityResult(result.Errors.Select(e => e.Description).ToList());
            }
            return new CustomIdentityResult(user.Id);
        }

        public async Task<string> GenerateRegistrationEncodedToken(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        }

        public async Task<ApiResponse<object>> ConfirmEmail(string email, string token)
        {
            var response = new ApiResponse<object>();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return response.setNotFoundResponse($"User with email {email} was not found.");
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return response.SetInternalServerErrorResponse();
            }
            return response;
        }

        public async Task<bool> UserEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<bool> UsernameExist(string name)
        {
            return await _userManager.FindByNameAsync(name) != null;
        }


        private async Task<AuthenticationResponse> GenerateAuthenticationResponseForUserAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                signingCredentials: signingCredentials);

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

        private ClaimsPrincipal GetPrincipalsFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
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

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                    jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture);
        }
    }
}
