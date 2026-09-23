using MediatR;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.CQRS.Command.Register
{
    public record RegisterCommand(string FullName,
        string Email,
        string Password,
        string ConfirmPassword) : IRequest<Microsoft.AspNetCore.Identity.IdentityResult>;
}
