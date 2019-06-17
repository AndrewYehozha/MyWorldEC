using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models.Response;

namespace WebApplication.Models.Request
{
    public class PaymentRequest
    {

        public int Id { get; set; }
        public int Entert_CenterId { get; set; }
        public string ServiceName { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<int> UserId { get; set; }
        public decimal Cost { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public bool IsBonusPayment { get; set; }
    }
}