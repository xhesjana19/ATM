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
    public class RechargeMap : BaseEntityTypeConfiguration<RechargeATM>
    {

        // <summary>
        /// Configurate
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<RechargeATM> builder)
        {
            builder.MapDefaults();
            builder.HasOne(x => x.ATM)
              .WithMany(other => other.RechargeATM)
              .HasForeignKey(x => x.AtmId)
              .OnDelete(DeleteBehavior.Restrict);
            builder.MapAuditableEntity();
            base.Configure(builder);
        }

        protected override void PostConfigure(EntityTypeBuilder<RechargeATM> builder)
        {
            base.PostConfigure(builder);
        }
    }
}
