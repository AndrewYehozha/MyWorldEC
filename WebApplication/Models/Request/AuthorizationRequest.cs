using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Request
{
    public class AuthorizationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}