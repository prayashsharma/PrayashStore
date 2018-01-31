using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PrayashStore.ViewModels
{
    public class RolesIndexViewModel
    {
        public RolesIndexViewModel()
        {
            RoleNames = new List<String>();
            UsersDropDownList = new List<SelectListItem>();
        }
        public IEnumerable<String> RoleNames { get; set; }
        public string SelectedUserName { get; set; }
        public IEnumerable<SelectListItem> UsersDropDownList { get; set; }
    }
}