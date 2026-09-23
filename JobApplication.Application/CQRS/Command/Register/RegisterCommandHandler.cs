using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace JobApplication.Application.CQRS.Command.Register
{
    public class RegisterCommandHandler :IRequestHandler<RegisterCommand,IdentityResult>
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;

        public RegisterCommandHandler(
            Microsoft.AspNetCore.Identity.UserManager
            <ApplicationUser> userManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<IdentityResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
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
    }
}
