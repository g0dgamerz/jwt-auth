using DataLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace POCOApp.Dependency
{
    public static class SeedData
    {
        public static async Task InitializeData(IHost app)
        {
            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

            using (var scope = scopedFactory.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetService<RoleManager<IdentityRole>>();
                var userManager = services.GetService<UserManager<ApplicationUser>>();
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                await roleManager.CreateAsync(role);


                if (userManager.FindByNameAsync("helloworld").Result == null)
                {
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = "helloworld";
                    user.Email = "testadmin@gmail.com";
                    user.PhoneNumber = "testadmin@gmail.com";

                    IdentityResult result = userManager.CreateAsync(user, "test@123").Result;

                    if (result.Succeeded)
                    {
                       userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
            }
        }
    }
}
