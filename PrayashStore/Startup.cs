using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using PrayashStore.Models;
using System.Web;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(PrayashStore.Startup))]
namespace PrayashStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // REGISTER DEPENDENCIES
            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<RoleStore<IdentityRole>>().As<IRoleStore<IdentityRole, string>>().InstancePerRequest();

            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();

            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            // REGISTER CONTROLLERS SO DEPENDENCIES ARE CONSTRUCTOR INJECTED
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // BUILD THE CONTAINER
            var container = builder.Build();

            // REPLACE THE MVC DEPENDENCY RESOLVER WITH AUTOFAC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // REGISTER WITH OWIN
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();


            ConfigureAuth(app);
            //CreateRolesandUsers();
        }
        private void CreateRolesandUsers()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                if (!roleManager.RoleExists("Admin"))
                {

                    // Create an Admin role   
                    var role = new IdentityRole()
                    {
                        Name = "Admin"
                    };
                    roleManager.Create(role);

                    // Create a Admin user who will maintain the website
                    var user = new ApplicationUser()
                    {
                        UserName = "admin@prayashstore.com",
                        Email = "admin@prayashstore.com",
                        EmailConfirmed = true,
                    };

                    var userPassword = "Admin-12345";
                    var result = UserManager.Create(user, userPassword);

                    //Add default User to Role Admin   
                    if (result.Succeeded)
                        UserManager.AddToRole(user.Id, "Admin");
                }


                // Create Other roles    
                if (!roleManager.RoleExists("CanManageProducts"))
                {
                    var role = new IdentityRole()
                    {
                        Name = "CanManageProducts"
                    };

                    roleManager.Create(role);

                }

                if (!roleManager.RoleExists("CanManageUsersAndRoles"))
                {
                    var role = new IdentityRole()
                    {
                        Name = "CanManageUsersAndRoles"
                    };

                    roleManager.Create(role);

                }
            }
        }
    }
}
