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

        [Required(ErrorMessage = "The Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        public Nullable<System.DateTime> Birsday { get; set; }
        public Nullable<System.DateTime> DateRegistered { get; set; }
        public Nullable<bool> IsBlocked { get; set; }
        public Nullable<bool> IsAdministration { get; set; }
        public Nullable<int> BonusScore { get; set; }
    }
}