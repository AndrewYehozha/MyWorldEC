﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class Discount_CardViewModel
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "The discount card number  is required")]
        [CreditCard(ErrorMessage = "Invalid discount card number")]
        public decimal NumberCard { get; set; }

        public string ServiceName { get; set; }

        public int? ServiceId { get; set; }
    }
}