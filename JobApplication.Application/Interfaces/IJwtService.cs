using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string email);
    }
}
