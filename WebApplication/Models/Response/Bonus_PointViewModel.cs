using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class Bonus_PointViewModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ServiceId { get; set; }
        public System.DateTime DateOfUse { get; set; }
        public decimal Amount { get; set; }
    }
}