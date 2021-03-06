﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Category Name is required")]
        public string Name { get; set; }
    }
}