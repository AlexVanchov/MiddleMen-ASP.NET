namespace MiddleMan.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using MiddleMan.Common;
    using MiddleMan.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRoleAsync(roleManager, userManager, GlobalConstants.AdministratorRoleName);
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.Users.AnyAsync())
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "Adminov",
                    ProfilePhotoUrl = "https://pm1.narvii.com/6240/334d3d792ec41be5053e14f7cbb101c5d0d8c64c_hq.jpg",
                };

                var password = "123456";

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
                }

                var user2 = new ApplicationUser
                {
                    UserName = "user",
                    Email = "user@gmail.com",
                    EmailConfirmed = true,
                };

                var password2 = "123456";

                var result2 = await userManager.CreateAsync(user2, password2);
            }
        }
    }
}
