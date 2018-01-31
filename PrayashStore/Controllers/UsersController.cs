using PrayashStore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PrayashStore.Controllers
{
    [Authorize(Roles = "CanManageUsersAndRoles,Admin")]
    public class UsersController : Controller
    {

        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;
        public UsersController()
        {

        }

        public UsersController(ApplicationRoleManager roleManager, ApplicationUserManager userManager)
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

        public ActionResult Index()
        {
            var users = UserManager.Users.Select(x => new { x.Id, x.UserName, x.PhoneNumber, x.Email }).ToList();

            var model = new UsersIndexViewModel()
            {
                Users = users.Select(x => new UserViewModel() { UserId = x.Id, UserName = x.UserName, Email = x.Email, PhoneNumber = x.PhoneNumber }).ToList(),
                UserResetPassword = new UserResetPasswordViewModel()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(UserResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.UserId);
                var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var result = await UserManager.ResetPasswordAsync(user.Id, token, model.Password);

                if (result.Succeeded)
                {
                    return Json(new { Success = true, Message = "Password Updated Successfully" });
                }
                else
                {
                    ModelState.AddModelError("", "Password Reset Failed!");
                }
            }
            string messages = string.Join("<br> ", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
            return Json(new { Success = false, Message = messages });
        }
    }
}