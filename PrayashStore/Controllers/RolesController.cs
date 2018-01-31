using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PrayashStore.Models;
using PrayashStore.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PrayashStore.Controllers
{
    [Authorize(Roles = "CanManageUsersAndRoles,Admin")]
    public class RolesController : Controller
    {
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;
        public RolesController()
        {

        }

        public RolesController(ApplicationRoleManager roleManager, ApplicationUserManager userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager;
            }
        }

        public ActionResult Index(RolesIndexViewModel model)
        {
            var roles = RoleManager.Roles.ToList();
            var users = UserManager.Users.ToList();

            IEnumerable<string> roleNames = new List<string>();
            if (string.IsNullOrWhiteSpace(model.SelectedUserName))
            {
                roleNames = roleNames = roles.Select(x => x.Name);
            }
            else
            {
                var user = UserManager.FindByName(model.SelectedUserName);
                if (user != null)
                    roleNames = UserManager.GetRoles(user.Id);
            }

            return View(GetRoles(roleNames, users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string roleName)
        {
            var role = RoleManager.FindByName(roleName);
            if (role != null)
            {
                var result = RoleManager.Delete(role);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Role deleted Successfully" });
                }
            }
            return Json(new { success = false, message = "Role delete failed" });
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RolesCreateViewModel roleCreateViewModel)
        {
            if (!ModelState.IsValid)
                return View(roleCreateViewModel);

            var result = RoleManager.Create(new IdentityRole(roleCreateViewModel.Name));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Errors.FirstOrDefault());
                return View(roleCreateViewModel);
            }

            return RedirectToAction("Index", "Roles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string userName, string roleName)
        {
            var user = UserManager.FindByName(userName);

            if (user == null)
            {
                return Json(new { success = false, message = "User Not found, Update failed" });
            }

            if (!UserManager.IsInRole(user.Id, roleName))
            {
                return Json(new { success = false, message = "User not present in current role, Update Failed" });
            }

            UserManager.RemoveFromRole(user.Id, roleName);
            return Json(new { success = true, message = "Role removed from User Successfully" });
        }

        public ActionResult AddRoleToUser()
        {
            return View(GetRolesAndUsersForDropDownList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoleToUser(RolesAndUsersDropDownViewModel model)
        {

            if (!ModelState.IsValid)
                return View(GetRolesAndUsersForDropDownList());

            var user = UserManager.FindByName(model.SelectedUserName);
            var role = RoleManager.FindByName(model.SelectedRoleName);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(GetRolesAndUsersForDropDownList());
            }

            if (role == null)
            {
                ModelState.AddModelError("", "Role not found");
                return View(GetRolesAndUsersForDropDownList());
            }

            if (UserManager.IsInRole(user.Id, model.SelectedRoleName))
            {
                ModelState.AddModelError("", "This user already has the role specified !");
                return View(GetRolesAndUsersForDropDownList());
            }

            UserManager.AddToRole(user.Id, role.Name);
            return RedirectToAction("Index", new RolesAndUsersDropDownViewModel { SelectedUserName = model.SelectedUserName });
        }

        private RolesAndUsersDropDownViewModel GetRolesAndUsersForDropDownList()
        {
            var roles = RoleManager.Roles.ToList();
            var users = UserManager.Users.ToList();

            var rolesAndUsers = new RolesAndUsersDropDownViewModel()
            {

                Roles = roles.Select(s => new SelectListItem
                {
                    Value = s.Name,
                    Text = s.Name
                }),

                Users = users.Select(s => new SelectListItem
                {
                    Value = s.UserName,
                    Text = s.UserName
                })

            };

            return (rolesAndUsers);
        }
        private RolesIndexViewModel GetRoles(IEnumerable<string> roleNames, List<ApplicationUser> users)
        {
            return new RolesIndexViewModel()
            {
                UsersDropDownList = users.Select(s => new SelectListItem
                {
                    Value = s.UserName,
                    Text = s.UserName
                }),
                RoleNames = roleNames
            };
        }
    }
}