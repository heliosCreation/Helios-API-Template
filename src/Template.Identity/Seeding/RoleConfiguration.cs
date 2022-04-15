using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Application.Enums;

namespace Template.Identity.Seeding
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        private const string superAdminId = "2301D884-221A-4E7D-B509-0113DCC043E1";
        private const string adminId = "7D9B7113-A8F8-4035-99A7-A20DD400F6A3";
        private const string moderatorId = "78A7570F-3CE5-48BA-9461-80283ED1D94D";
        private const string basicId = "01B168FE-810B-432D-9010-233BA0B380E9";

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(new IdentityRole
            {
                Id = basicId,
                Name = Roles.Basic.ToString(),
                NormalizedName = Roles.Basic.ToString().ToUpper()
            });

            builder.HasData(new IdentityRole
            {
                Id = moderatorId,
                Name = Roles.Moderator.ToString(),
                NormalizedName = Roles.Moderator.ToString().ToUpper()
            });

            builder.HasData(new IdentityRole
            {
                Id = adminId,
                Name = Roles.Admin.ToString(),
                NormalizedName = Roles.Admin.ToString().ToUpper()
            });

            builder.HasData(new IdentityRole
            {
                Id = superAdminId,
                Name = Roles.SuperAdmin.ToString(),
                NormalizedName = Roles.SuperAdmin.ToString().ToUpper()
            });
        }
    }
}
