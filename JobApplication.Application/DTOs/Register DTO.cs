using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.DTOs
{
    public class Register_DTO
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; 
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
