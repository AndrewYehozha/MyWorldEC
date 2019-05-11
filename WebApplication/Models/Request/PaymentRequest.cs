using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models.Response;

namespace WebApplication.Models.Request
{
    public class PaymentRequest : PaymentViewModel
    {
        public bool IsBonusPayment { get; set; }
    }
}