using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class ServicesViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Service Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The Cost is required")]
        public Nullable<decimal> Cost { get; set; }
        public Nullable<int> Floor { get; set; }
        public Nullable<int> Hall { get; set; }
    }
}