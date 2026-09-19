using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.DTOs
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; 
        public string Token { get; set; } = string.Empty;
    }
}
