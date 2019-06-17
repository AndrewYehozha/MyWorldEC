using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class ServiceCategoryViewModel : ServicesViewModel
    {
        public virtual IEnumerable<CategoryViewModel> Categories { get; set; }
        public decimal Rating { get; set; }
        public decimal? userRating { get; set; }
    }
}