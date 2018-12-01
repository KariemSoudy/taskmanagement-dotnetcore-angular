using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Linq;
using TasksManagement.Data.Entities;

namespace TasksManagement.Data
{
    public class DBInitializer
    {
        public static async System.Threading.Tasks.Task SeedUsersDataAsync(TaskManagementDBContext context, UserManager<User> userManager)
        {
            var user1Task = await CreateUserAsync(context, userManager, new User
            {
                Email = "admin1@example.com",
                NormalizedEmail = "ADMIN1@EXAMPLE.COM",
                UserName = "admin1",
                NormalizedUserName = "ADMIN1",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = DateTime.Now.ToString("ddMMyyyy")
            }, "123456", "Admin");

            if (user1Task.Succeeded)
            {
                var user2Task = await CreateUserAsync(context, userManager, new User
                {
                    Email = "support1@example.com",
                    NormalizedEmail = "SUPPORT1@EXAMPLE.COM",
                    UserName = "support1",
                    NormalizedUserName = "SUPPORT1",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = DateTime.Now.ToString("ddMMyyyy")
                }, "123456", "Support");

                if (user2Task.Succeeded)
                {
                    await CreateUserAsync(context, userManager, new User
                    {
                        Email = "support2@example.com",
                        NormalizedEmail = "SUPPORT2@EXAMPLE.COM",
                        UserName = "support2",
                        NormalizedUserName = "SUPPORT2",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = DateTime.Now.ToString("ddMMyyyy")
                    }, "123456", "Support");
                }
            }
        }

        public static async System.Threading.Tasks.Task<IdentityResult> CreateUserAsync(TaskManagementDBContext context, UserManager<User> userManager, User user, string password, string role)
        {
            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var passwordHasher = new PasswordHasher<User>();
                var hashed = passwordHasher.HashPassword(user, password);
                user.PasswordHash = hashed;

                var userStore = new UserStore<User>(context);
                var result = await userStore.CreateAsync(user);

                if (result.Succeeded)
                    context.SaveChangesAsync().Wait();

            }

            var assignRole = await AssignRoles(userManager, user.Email, role);

            if (assignRole.Succeeded)
                context.SaveChangesAsync().Wait();

            return assignRole;
        }

        public static async System.Threading.Tasks.Task<IdentityResult> AssignRoles(UserManager<User> userManager, string email, string role)
        {
            User user = await userManager.FindByEmailAsync(email);
            var result = await userManager.AddToRoleAsync(user, role);

            return result;
        }

    }
}
