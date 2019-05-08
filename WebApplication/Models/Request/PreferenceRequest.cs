using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Request
{
    public class PreferenceRequest
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> ServiceId { get; set; }
    }
}