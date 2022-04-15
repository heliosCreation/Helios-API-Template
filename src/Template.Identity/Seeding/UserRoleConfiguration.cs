using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Template.Identity.Seeding
{
    public class UsersWithRolesConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        private const string superAdminId = "f1aafc30-5e54-4550-a5a1-4df0704b3258";
        private const string basicUserId = "9f93e84d-bcfe-4dab-9ba7-cb7065a63524";

        private const string superAdminRoleId = "2301D884-221A-4E7D-B509-0113DCC043E1";
        private const string basicRoleId = "01B168FE-810B-432D-9010-233BA0B380E9";

        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            IdentityUserRole<string> iurAdmin = new IdentityUserRole<string>
            {
                RoleId = superAdminRoleId,
                UserId = superAdminId
            };

            IdentityUserRole<string> iur = new IdentityUserRole<string>
            {
                RoleId = basicRoleId,
                UserId = basicUserId
            };

            builder.HasData(iur);
            builder.HasData(iurAdmin);
        }
    }
}
