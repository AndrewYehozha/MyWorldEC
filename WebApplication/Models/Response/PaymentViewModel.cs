using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public int Entert_CenterId { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<int> UserId { get; set; }
        public decimal Cost { get; set; }
        public System.DateTime PaymentDate { get; set; }
    }
}