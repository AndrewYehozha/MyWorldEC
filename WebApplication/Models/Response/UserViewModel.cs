using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The Surname is required")]
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+)?(\d{10})(\d{1,4})?$", ErrorMessage = "Invalid Phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        public DateTime? Birsday { get; set; }
        public DateTime DateRegistered { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsAdministration { get; set; }
        public int BonusScore { get; set; }
    }
}