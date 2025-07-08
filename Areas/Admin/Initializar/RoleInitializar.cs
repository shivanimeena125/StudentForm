using Formio.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Formio.Areas.Admin.Initializar
{
    public class RoleInitializar
    {
        public static async Task RoleInitializarAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

          
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            
            var adminEmail = "test123@gmail.com"; 
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser != null)
            {
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            else
            {
                Console.WriteLine($"Admin user with email {adminEmail} not found. Please register this user first.");
            }
            var allUsers = userManager.Users.ToList();

            foreach (var user in allUsers)
            {
                if (user.Email != adminEmail && !await userManager.IsInRoleAsync(user, "User"))
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }

    }
}
