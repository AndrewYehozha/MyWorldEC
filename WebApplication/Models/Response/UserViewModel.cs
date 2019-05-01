using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> Birsday { get; set; }
        public Nullable<System.DateTime> DateRegistered { get; set; }
        public Nullable<bool> IsBlocked { get; set; }
        public Nullable<bool> IsAdministration { get; set; }
        public Nullable<int> BonusScore { get; set; }
    }
}