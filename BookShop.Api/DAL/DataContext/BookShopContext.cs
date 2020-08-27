using BookShop.Api.DAL.Models;
using BookShop.Api.DAL.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace BookShop.Api.DAL.DataContext
{
    public class BookShopContext : IdentityDbContext<User, Role, Guid,
        IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Order> Orders { get; set; }

        public BookShopContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(user =>
            {
                user.Ignore(u => u.TwoFactorEnabled)
                    .Ignore(u => u.LockoutEnabled)
                    .Ignore(u => u.LockoutEnd)
                    .Ignore(u => u.AccessFailedCount);

                user.Property(u => u.Email).HasMaxLength(30)
                                           .IsRequired();
                user.Property(u => u.UserName).HasMaxLength(30)
                                              .IsRequired();

                //migration builder requires using Fluent Api to set table name for classes inherited from Identity package(Table attribute will not work)
                user.ToTable("User");
            });

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                userRole.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

                userRole.ToTable("UserRole");
            });

            builder.Entity<Role>(role =>
            {
                role.ToTable("Role");
            });

            builder.Entity<RefreshToken>(token => 
            {
                token.HasIndex(t => new { t.DeviceName, t.UserId }).IsUnique();

                token.HasOne(t => t.User)
                     .WithMany(u => u.RefreshTokens)
                     .HasForeignKey(t => t.UserId)
                     .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Order>(order => 
            {
                order.HasOne(o => o.User)
                     .WithMany(u => u.Orders)
                     .HasForeignKey(o => o.UserId)
                     .OnDelete(DeleteBehavior.Restrict);

                order.HasOne(o => o.Book)
                     .WithMany(b => b.Orders)
                     .HasForeignKey(o => o.BookId)
                     .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Ignore<IdentityUserClaim<Guid>>();
            builder.Ignore<IdentityUserLogin<Guid>>();
            builder.Ignore<IdentityRoleClaim<Guid>>();
            builder.Ignore<IdentityUserToken<Guid>>();
        }
    }
}
