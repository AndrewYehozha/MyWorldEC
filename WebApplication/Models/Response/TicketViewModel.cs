using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public int? UserId { get; set; }
        public System.DateTime PreOrder_Date { get; set; }
        public decimal Price { get; set; }
        public bool IsUse { get; set; }
    }
}