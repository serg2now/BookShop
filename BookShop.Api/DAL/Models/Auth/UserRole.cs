using System;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Api.DAL.Models.Auth
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }
}
