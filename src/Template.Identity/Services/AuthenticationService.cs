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
using Template.Application.Responses;
using Template.Identity.Models;

namespace Template.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppIdentityDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            AppIdentityDbContext context)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _context = context ?? throw new ArgumentNullException(nameof(context));
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

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            response.Data = new AuthenticationResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName
            };

            return response;
        }

        //public async Task<ApiResponse<RegistrationResponse>> RegisterAsync(RegistrationRequest request)
        //{
        //    var response = new ApiResponse<RegistrationResponse>();
        //    var existingUser = await _userManager.FindByNameAsync(request.UserName);

        //    if (existingUser != null)
        //    {
        //        return response.SetBadRequestResponse($"Username {request.UserName} is already taken.");
        //    }

        //    var user = new ApplicationUser
        //    {
        //        Email = request.Email,
        //        FirstName = request.FirstName,
        //        LastName = request.LastName,
        //        UserName = request.UserName,
        //        EmailConfirmed = false
        //    };

        //    var existingEmail = await _userManager.FindByEmailAsync(request.Email);

        //    if (existingEmail == null)
        //    {
        //        var result = await _userManager.CreateAsync(user, request.Password);

        //        if (result.Succeeded)
        //        {
        //            response.Data = new RegistrationResponse() { UserId = user.Id };
        //            return response;
        //        }
        //        else
        //        {
        //            return response.SetBadRequestResponse(null, result.Errors.Select(e => e.Description).ToList());
        //        }
        //    }
        //    else
        //    {
        //        return response.SetBadRequestResponse($"Email {request.Email} is already taken.");
        //    }
        //}

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


        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
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
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

    }
}
