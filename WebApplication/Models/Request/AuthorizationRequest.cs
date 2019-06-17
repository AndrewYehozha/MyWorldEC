using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Request
{
    public class AuthorizationRequest
    {
        [Required(ErrorMessage = "The Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your Password.")]
        [StringLength(50, MinimumLength = 7, ErrorMessage = "Password should be longer than 6 characters.")]
        public string Password { get; set; }
    }
}