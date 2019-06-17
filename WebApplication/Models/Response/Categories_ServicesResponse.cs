using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class Categories_ServicesResponse
    {
        public Nullable<int> IdCategories { get; set; }
        public Nullable<int> IdServices { get; set; }
        public int Id { get; set; }

        public CategoryViewModel Category { get; set; }
        public ServicesViewModel Service { get; set; }
    }
}