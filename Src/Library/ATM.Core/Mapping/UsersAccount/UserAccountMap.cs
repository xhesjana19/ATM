using ATM.Core.Data.Extensions;
using ATM.Core.Data.Mapping;
using ATM.Core.Domain.Users;
using ATM.Core.Domain.UsersAccount;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Mapping.UsersAccount
{
    /// <summary>
    /// User Account configuration
    /// </summary>
    public class UserAccountMap : BaseEntityTypeConfiguration<UserAccount>
    {
        // <summary>
        /// Configurate
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.MapDefaults();
              builder.HasOne(x => x.User)
                .WithMany(other => other.UsersAccount)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.MapAuditableEntity();
            base.Configure(builder);
        }

        protected override void PostConfigure(EntityTypeBuilder<UserAccount> builder)
        {
            base.PostConfigure(builder);
        }
    }
}
