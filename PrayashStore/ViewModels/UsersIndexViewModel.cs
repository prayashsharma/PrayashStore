using System.Collections.Generic;

namespace PrayashStore.ViewModels
{
    public class UsersIndexViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public UserResetPasswordViewModel UserResetPassword { get; set; }
    }
}