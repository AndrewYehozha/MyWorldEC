using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class ServicesToECResponse
    {
        public Nullable<int> ServiceId { get; set; }
        public int Id { get; set; }
        public Nullable<int> Entertainment_CenterId { get; set; }

        public Entertainment_CentersViewModel Entertainment_Centers { get; set; }
        public ServicesViewModel Service { get; set; }
    }
}