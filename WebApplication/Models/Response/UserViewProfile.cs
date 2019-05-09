using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Response
{
    public class UserViewProfile : UserViewModel
    {
        public List<PreferenceViewModel> Preferences { get; set; }
        public List<Discount_CardViewModel> DiscountCards { get; set; }
        public List<ChildrenViewModel> Childrens { get; set; }
    }
}