using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public string Entert_CenterName { get; set; }
        public string ServiceName { get; set; }
        public string UserEmail { get; set; }
        public decimal Cost { get; set; }
        public System.DateTime PaymentDate { get; set; }
    }
}