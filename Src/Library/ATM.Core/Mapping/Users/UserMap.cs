using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ATM.Core.Data.Extensions;
using ATM.Core.Data.Mapping;
using ATM.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ATM.Core.Mapping.Users
{
    /// <summary>
    /// User configuration
    /// </summary>
    public class UserMap : BaseEntityTypeConfiguration<User>
    {
        /// <summary>
        /// Configurate
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.MapDefaults();

            builder.Property(mapping => mapping.Name).IsRequired().HasMaxLength(128);
            builder.Property(mapping => mapping.Username).IsRequired().HasMaxLength(128);
            builder.Property(mapping => mapping.Password).IsRequired().HasMaxLength(128);
            builder.Property(mapping => mapping.Role).IsRequired().HasMaxLength(64);

            base.Configure(builder);
        }

        protected override void PostConfigure(EntityTypeBuilder<User> builder)
        {
            base.PostConfigure(builder);
        }
    }
}
