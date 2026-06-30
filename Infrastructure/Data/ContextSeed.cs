using ClinicManagement.API.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ClinicManagement.API.Infrastructure.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager )
        {
            string[] roles = { "Admin", "Doctor", "Patient", "Rexeptionist" };
            foreach(var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
           
        }
        public static async Task CreateAdmin(UserManager<User> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@clinic.com") is null)
            {
                string password = "Admin@123";
                var user = new User
                {
                    UserName = "Admin",
                    Email = "admin@clinic.com"
                };
                var saveAdd = await userManager.CreateAsync(user, password);
                if(saveAdd.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
