using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Contracts.Identity;
using Template.Application.Features.Account;
using Template.Application.Features.Account.Command.Authenticate;
using Template.Application.Features.Account.Command.ConfirmEmail;
using Template.Application.Features.Account.Command.RefreshToken;
using Template.Application.Features.Account.Command.Register;
using Template.Application.Features.Account.Command.RegistrationToken;
using Template.Application.Features.Account.Command.ResetPassword;
using Template.Application.Model.Account;
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


        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticateCommand request)
        {
            var response = new AuthenticationResponse();
            var user = await _context.Users.Where(u => u.Email == request.Email).FirstOrDefaultAsync();
            if (user == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "No user found that was associated to this email.";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                response.IsSuccess = false;
                return response;
            }

            response = await _tokenUtils.GenerateAuthenticationResponseForUserAsync(user.Id, _jwtSettings);

            return response;
        }
        public async Task<AuthenticationResponse> RefreshTokenAsync(ResfreshTokenCommand request)
        {
            var response = new AuthenticationResponse();

            var claimPrincipals = _tokenUtils.GetPrincipalsFromToken(request.Token, _tokenValidationParameters);
            if (claimPrincipals == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Invalid Token";
                return response;
            }

            if (!_tokenUtils.JwtIsExpired(claimPrincipals))
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Token hasn't expired yet.";
                return response;
            }

            var jti = claimPrincipals.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == request.RefreshToken);

            response = _tokenUtils.ValidateDbRefreshToken(storedRefreshToken, jti);
            if (response.ErrorMessage != null)
            {
                response.IsSuccess = false;
                return response;
            }

            storedRefreshToken.IsUsed = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(claimPrincipals.Claims.Single(c => c.Type == "uid").Value);
            response = await _tokenUtils.GenerateAuthenticationResponseForUserAsync(user.Id, _jwtSettings);

            return response;
        }
        public async Task<RegistrationResponse> RegisterAsync(RegisterCommand command)
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
                return new RegistrationResponse(result.Errors.Select(e => e.Description).ToList());
            }
            return new RegistrationResponse(user.Id);
        }
        public async Task<RegistrationTokenResponse> GenerateRegistrationEncodedToken(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new RegistrationTokenResponse(error: $"User with ID {id} was not found");
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return new RegistrationTokenResponse() { Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)) };
        }
        public async Task<ApiResponse<object>> ConfirmEmail(ConfirmEmailCommand request)
        {
            var response = new ApiResponse<object>();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return response.setNotFoundResponse($"User with email {request.Email} was not found.");
            }
            var decodedTokenString = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.RegistrationToken));
            var result = await _userManager.ConfirmEmailAsync(user, decodedTokenString);
            if (!result.Succeeded)
            {
                return response.SetBadRequestResponse(message: "There was an error with the provided registration token.");
            }
            return response;
        }
        public async Task<string> GeneratePasswordForgottenMailToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (!user.EmailConfirmed)
            {
                return null;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }
        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordCommand request)
        {
            var user = await _userManager.FindByIdAsync(request.Uid);
            var result =  await _userManager.ResetPasswordAsync(user, request.ResetToken, request.NewPassword);
            if (!result.Succeeded)
            {
                return new ResetPasswordResponse("The combination UID/TOKEN was wrong");
            }
            return new ResetPasswordResponse(); 
        }

        public async Task<string> GetUserIdAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null ? user.Id : null;
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
