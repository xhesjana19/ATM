using ATM.Core.Data.Extensions;
using ATM.Core.Data.Mapping;
using ATM.Core.Domain.ATMs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Mapping.ATMs
{
    /// <summary>
    /// User Account configuration
    /// </summary>
    public class ATMWithdrawalsMap : BaseEntityTypeConfiguration<ATMWithdrawals>
    {
        // <summary>
        /// Configurate
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<ATMWithdrawals> builder)
        {
            builder.MapDefaults();
            builder.HasOne(x => x.ATM)
              .WithMany(other => other.ATMWithdrawals)
              .HasForeignKey(x => x.AtmId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Users)
              .WithMany(other => other.ATMWithdrawals)
              .HasForeignKey(x => x.UserId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.MapAuditableEntity();
            base.Configure(builder);
        }

        protected override void PostConfigure(EntityTypeBuilder<ATMWithdrawals> builder)
        {
            base.PostConfigure(builder);
        }
    }
}
