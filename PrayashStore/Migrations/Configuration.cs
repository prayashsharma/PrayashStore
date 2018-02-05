namespace PrayashStore.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var roleName = "Admin";
            var userName = "admin@prayashstore.com";
            var userPassword = "Admin-12345";

            if (!roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole()
                {
                    Name = roleName
                };
                roleManager.Create(role);
            }
            if (userManager.FindByName(userName) == null)
            {
                var newUser = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true,
                };

                userManager.Create(newUser, userPassword);
            }

            var user = userManager.FindByName(userName);
            if (!string.IsNullOrEmpty(user.Id))
            {
                if (!userManager.CheckPassword(user, userPassword))
                {
                    var dataProtectionProvider = Startup.DataProtectionProvider;
                    userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("PasswordReset"));
                    var token = userManager.GeneratePasswordResetToken(user.Id);
                    userManager.ResetPassword(user.Id, token, userPassword);
                }

                if (!userManager.IsInRole(user.Id, roleName))
                    userManager.AddToRole(user.Id, roleName);
            }
        }
    }
}
