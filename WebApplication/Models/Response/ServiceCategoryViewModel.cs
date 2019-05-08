using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class ServiceCategoryViewModel : ServicesViewModel
    {
        public virtual IEnumerable<Category> Categories { get; set; }
        public decimal? Rating { get; set; }
    }
}