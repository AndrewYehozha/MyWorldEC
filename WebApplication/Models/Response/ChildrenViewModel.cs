using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class ChildrenViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The Surname is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The Date of Birthday is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid Date of Birthday")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
    }
}