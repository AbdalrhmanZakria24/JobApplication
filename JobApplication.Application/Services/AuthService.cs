using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace JobApplication.Application.Services
{
    public class AuthService
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthService(
            Microsoft.AspNetCore.Identity.UserManager
            <ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> RegisterAsync(
            Register_DTO request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return Microsoft.AspNetCore.Identity.IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Passwords do not match."
                    });
            }

            var existingUser =
                await _userManager.FindByEmailAsync(request.Email);

            if (existingUser is not null)
            {
                return Microsoft.AspNetCore.Identity.IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Email is already registered."
                    });
            }

            var user = new ApplicationUser
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(
                user,
                request.Password);

            return result;
        }

        public async Task<LoginResponse?> LoginAsync(
            LoginRequest request)
        {
            var user =
                await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return null;
            }

            if (!user.IsActive)
            {
                return null;
            }

            var result = await _signInManager
                .CheckPasswordSignInAsync(
                    user,
                    request.Password,
                    lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                return null;
            }

            var token = _jwtService.GenerateToken(
                user.Id,
                user.Email!);

            return new LoginResponse
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                Token = token
            };
        }
    }
}
