using ATM.Core.Data.Extensions;
using ATM.Core.Data.Mapping;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Mapping.ATMs
{
    /// <summary>
    /// ATM configuration
    /// </summary>
    public class ATMMap : BaseEntityTypeConfiguration<Domain.ATMs.ATM>
    {
        /// <summary>
        /// Configurate
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<Domain.ATMs.ATM> builder)
        {
            builder.MapDefaults();

            builder.Property(mapping => mapping.Name).IsRequired().HasMaxLength(128);
            builder.MapAuditableEntity();

            base.Configure(builder);
        }

        protected override void PostConfigure(EntityTypeBuilder<Domain.ATMs.ATM> builder)
        {
            base.PostConfigure(builder);
        }
    }
}
