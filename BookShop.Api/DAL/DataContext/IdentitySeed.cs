using BookShop.Api.DAL.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.DAL.DataContext
{
    public class IdentitySeed
    {
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;
        private IConfiguration _configuration; 

        public IdentitySeed(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task Seed()
        {
            if (!_userManager.Users.Any())
            {
                var roles = new List<Role>
                {
                    new Role{Name = "Client"},
                    new Role{Name = "Admin"}
                };

                roles.ForEach(r =>  _roleManager.CreateAsync(r).Wait());

                User admin = new User()
                {
                    Name = "Admin",
                    UserName = "Admin",
                    Surname = "",
                    Email = "admin@admin",
                    BirthDate = DateTime.MinValue,
                    City = "Kyiv",
                    DeliveryAdress = "n/a",
                    PhoneNumber = "n/a"
                };

                var adminPass = _configuration.GetSection("Security:adminPassword").Value;
                var result = await _userManager.CreateAsync(admin, adminPass);

                if (result.Succeeded)
                {
                    User user = await _userManager.FindByNameAsync("Admin");
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
