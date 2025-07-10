using Formio.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Formio.Areas.Identity.RoleInitilizar
{
    public static class AssignRole
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "User", "Editor" };

           
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

           
            var allUsers = userManager.Users.ToList();

           
            var adminEmail = "aaa@gmail.com"; 
            var adminUser = allUsers.FirstOrDefault(u => u.Email == adminEmail);

            if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

          
            foreach (var user in allUsers)
            {
                if (user.Id != adminUser?.Id && !await userManager.IsInRoleAsync(user, "User"))
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}
