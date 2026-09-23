using JobApplication.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.CQRS.Command.Login
{
    public  record LoginCommand(string Email,
         string Password) : IRequest<LoginResponse>;
}
