using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Request
{
    public class PreferenceRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
    }
}