using System;
using System.Collections.Generic;
using BookShop.Api.DAL.Models.Base;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Api.DAL.Models.Auth
{
    public class Role : IdentityRole<Guid>, IModel
    {
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
