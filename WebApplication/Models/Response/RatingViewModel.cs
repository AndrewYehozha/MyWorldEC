using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class RatingViewModel
    {
        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public int? UserId { get; set; }

        [Required(ErrorMessage = "The Rating is required")]
        public decimal? Rating1 { get; set; }
    }
}