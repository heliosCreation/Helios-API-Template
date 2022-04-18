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
using Template.Application.Features.Account.Command.Register;
using Template.Application.Model.Account;
using Template.Application.Model.Account.Authentification;
using Template.Application.Models.Account.RefreshToken;
using Template.Application.Responses;
using Template.Identity.Entities;

namespace Template.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenUtils _tokenUtils;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppIdentityDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;


        public AuthenticationService(
            ITokenUtils tokenUtils,
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            AppIdentityDbContext context,
            TokenValidationParameters tokenValidationParameters)
        {
            _tokenUtils = tokenUtils ?? throw new ArgumentNullException(nameof(tokenUtils));
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

            response.Data = await _tokenUtils.GenerateAuthenticationResponseForUserAsync(user.Id, _jwtSettings);

            return response;
        }
        public async Task<ApiResponse<AuthenticationResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var response = new ApiResponse<AuthenticationResponse>();

            var claimPrincipals = _tokenUtils.GetPrincipalsFromToken(request.Token, _tokenValidationParameters);
            if (claimPrincipals == null)
            {
                return response.SetBadRequestResponse(message: "Invalid Token");
            }

            if (!_tokenUtils.JwtIsExpired(claimPrincipals))
            {
                return response.SetBadRequestResponse(message: "Token hasn't expired yet.");
            }

            var jti = claimPrincipals.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == request.RefreshToken);

            response = _tokenUtils.ValidateDbRefreshToken(storedRefreshToken, jti);
            if (!response.Succeeded)
            {
                return response;
            }

            storedRefreshToken.IsUsed = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(claimPrincipals.Claims.Single(c => c.Type == "uid").Value);
            response.Data =  await _tokenUtils.GenerateAuthenticationResponseForUserAsync(user.Id, _jwtSettings);

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

    }
}
