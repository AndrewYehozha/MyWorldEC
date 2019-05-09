using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Request
{
    public class Services_Entertainment_CentersRequest
    {
        public int ServiceId { get; set; }
        public int Id { get; set; }
        public int Entertainment_CenterId { get; set; }
    }
}