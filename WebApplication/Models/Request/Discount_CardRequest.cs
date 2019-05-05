using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models.Response;

namespace WebApplication.Models.Request
{
    public class Discount_CardRequest : Discount_CardViewModel
    {
        public int UserId { get; set; }
    }
}