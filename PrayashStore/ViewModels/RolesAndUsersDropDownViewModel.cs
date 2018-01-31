using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PrayashStore.ViewModels
{
    public class RolesAndUsersDropDownViewModel
    {
        public RolesAndUsersDropDownViewModel()
        {
            Roles = new List<SelectListItem>();
            Users = new List<SelectListItem>();
        }

        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Role is required")]
        public string SelectedRoleName { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "User is required")]
        public string SelectedUserName { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }

    }
}