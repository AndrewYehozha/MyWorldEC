using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class Entertainment_CentersViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name Entetainment Center is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Owner is required")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "The Address is required")]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+)?(\d{9})(\d{1,4})?$", ErrorMessage = "Invalid Phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        public bool IsParking { get; set; }
    }
}