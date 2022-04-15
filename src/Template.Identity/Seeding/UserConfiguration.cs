using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Template.Identity.Models;

namespace Template.Identity.Seeding
{
    public class UsersConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        private const string superAdminId = "f1aafc30-5e54-4550-a5a1-4df0704b3258";
        private const string basicUserId = "9f93e84d-bcfe-4dab-9ba7-cb7065a63524";


        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var superAdmin = new ApplicationUser
            {
                Id = superAdminId,
                UserName = "masteradmin",
                NormalizedUserName = "MASTERADMIN",
                FirstName = "Master",
                LastName = "Admin",
                Email = "SuperAdmin@Admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                PhoneNumber = "XXXXXXXXXXXXX",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = new Guid().ToString(),
            };

            var defaultUser = new ApplicationUser
            {
                Id = basicUserId,
                UserName = "John",
                NormalizedUserName = "JOHN",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@gmail.com",
                NormalizedEmail = "JOHN@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = new Guid().ToString(),
            };

            superAdmin.PasswordHash = PassGenerate(superAdmin);
            defaultUser.PasswordHash = PassGenerate(defaultUser);

            builder.HasData(superAdmin);
            builder.HasData(defaultUser);
        }

        public string PassGenerate(ApplicationUser user)
        {
            var passHash = new PasswordHasher<ApplicationUser>();
            return passHash.HashPassword(user, "Pwd12345!");
        }
    }
}
