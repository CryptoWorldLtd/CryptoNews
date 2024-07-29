using CryptoWorld.News.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using CryptoWorld.News.Data.Models.Common;
using System;

namespace CryptoWorld.News.Data.Seeding
{
    public class Seeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedRoleAsync(roleManager);
            await UserSeeder(userManager);
        }
        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager)
        {
            string[] roles = { GlobalConstants.AdminRoleName, GlobalConstants.AuthorRoleName, GlobalConstants.EditorRoleName, GlobalConstants.GuestRoleName };
            IdentityResult result;
            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                     result = await roleManager.CreateAsync(new ApplicationRole(role));
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }
        private static async Task UserSeeder(UserManager<ApplicationUser> userManager)
        {
            var adminUser = new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "Testov",
                UserName = "admin@gmail.com",
                NormalizedUserName = "admin@gmail.com".ToUpper(),
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com".ToUpper(),
                Age = 24,
                Gender = "Male",
                DateOfBirth = DateTime.Now.AddYears(-24),
                CreatedOn = DateTime.Now,
                EmailConfirmed = true,
                PhoneNumber = "359882312345"
            };

            string adminPassword = "Admin@123";
            var admin = await userManager.FindByEmailAsync("admin@gmail.com");

            if (admin == null)
            {
                var createAdmin = await userManager.CreateAsync(adminUser, adminPassword);
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            var editorUser = new ApplicationUser
            {
                FirstName = "Editor",
                LastName = "Testov",
                UserName = "editor@gmail.com",
                NormalizedUserName = "editor@gmail.com".ToUpper(),
                Email = "editor@gmail.com",
                NormalizedEmail = "editor@gmail.com".ToUpper(),
                Age = 36,
                Gender = "Male",
                DateOfBirth = DateTime.Now.AddYears(-36),
                CreatedOn = DateTime.Now,
                EmailConfirmed = true,
                PhoneNumber = "359882354345"
            };

            string editorPassword = "Editor@123";
            var editor = await userManager.FindByEmailAsync("editor@gmail.com");

            if (editor == null)
            {
                var createEditor = await userManager.CreateAsync(editorUser, editorPassword);
                if (createEditor.Succeeded)
                {
                    await userManager.AddToRoleAsync(editorUser, "Editor");
                }
            }

            var authorUser = new ApplicationUser
            {
                FirstName = "Author",
                LastName = "Testov",
                UserName = "author@gmail.com",
                NormalizedUserName = "author@gmail.com".ToUpper(),
                Email = "author@gmail.com",
                NormalizedEmail = "author@gmail.com".ToUpper(),
                Age = 46,
                Gender = "Female",
                DateOfBirth = DateTime.Now.AddYears(-46),
                CreatedOn = DateTime.Now,
                EmailConfirmed = true,
                PhoneNumber = "359882312005"
            };

            string authorPassword = "Author@123";
            var author = await userManager.FindByEmailAsync("author@gmail.com");

            if (author == null)
            {
                var createAuthor = await userManager.CreateAsync(authorUser, authorPassword);
                if (createAuthor.Succeeded)
                {
                    await userManager.AddToRoleAsync(authorUser, "Author");
                }
            }

            var guestUser = new ApplicationUser
            {
                FirstName = "Guest",
                LastName = "Testov",
                UserName = "guest@example.com",
                Email = "guest@example.com",
                NormalizedUserName = "guest@gmail.com".ToUpper(),
                NormalizedEmail = "guest@gmail.com".ToUpper(),
                Age = 24,
                Gender = "Male",
                DateOfBirth = DateTime.Now.AddYears(-24),
                CreatedOn = DateTime.Now,
                EmailConfirmed = true,
                PhoneNumber = "359882354321"
            };

            string userPassword = "GuestUser@123";
            var guest = await userManager.FindByEmailAsync("guest@example.com");

            if (guest == null)
            {
                var createGuest = await userManager.CreateAsync(guestUser, userPassword);
                if (createGuest.Succeeded)
                {
                    await userManager.AddToRoleAsync(guestUser, "Guest");
                }
            }
        }
    }
}
