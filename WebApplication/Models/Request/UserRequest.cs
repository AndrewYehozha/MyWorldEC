﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models.Response;

namespace WebApplication.Models.Request
{
    public class UserRequest : UserViewModel
    {
        public string Password { get; set; }
    }
}