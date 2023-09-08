using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Seed Roles (User,Admin,SuperAdmin)

            var adminRoleId = "f7ff9892-774b-4ddf-934b-72ba230bd0d3";
            var SuperadminRoleId = "da8e94db-db81-4092-b651-f1c885b0ef04";
            var userRoleId = "66f0c1a8-683a-4d39-b9af-a321779b67db";


            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                },

                new IdentityRole
                {
                     Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = SuperadminRoleId,
                    ConcurrencyStamp = SuperadminRoleId,
                },
                new IdentityRole
                {
                     Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //Seed SuperAdminUser
            var SuperAdminUserId = "319e3ab4-0c13-431f-9743-f1fd02741343";
            var SuperAdminUser = new IdentityUser
            {
                UserName = "SuperAdmin@bloggie.com",
                Email = "SuperAdmin@bloggie.com",
                NormalizedEmail = "SuperAdmin@bloggie.com".ToUpper(),
                NormalizedUserName = "SuperAdmin@bloggie.com".ToUpper(),
                Id = SuperAdminUserId
            };

            SuperAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(SuperAdminUser, "SuperAdmin@123");

            builder.Entity<IdentityUser>().HasData(SuperAdminUser);


            //Add All The roles to SuperAdminUser

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                //admin
                new IdentityUserRole<string>
                {
                     RoleId = adminRoleId,
                     UserId = SuperAdminUserId,
                },
                
                //SuperAdmin
                new IdentityUserRole<string>
                {
                     RoleId = SuperadminRoleId,
                     UserId = SuperAdminUserId,
                },

                //User
                new IdentityUserRole<string>
                {
                     RoleId = userRoleId,
                     UserId = SuperAdminUserId,
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);   
        }
    }
}
