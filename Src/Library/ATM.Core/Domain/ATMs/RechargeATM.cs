using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Domain.ATMs
{
    public class RechargeATM : AuditableEntity
    {
        /// <summary>
        /// Gets and Sets Banknotes 5000
        /// </summary>
        public int? Banknotes5000 { get; set; }

        /// <summary>
        /// Gets and Sets Banknotes 2000
        /// </summary>
        public int? Banknotes2000 { get; set; }

        /// <summary>
        /// Gets and Sets Banknotes 1000
        /// </summary>
        public int? Banknotes1000 { get; set; }

        /// <summary>
        /// Gets and Sets Banknotes 500 
        /// </summary>
        public int? Banknotes500 { get; set; }

        /// <summary>
        /// Gets or sets the user
        /// </summary>
        public Guid AtmId { get; set; }
        public virtual ATM ATM { get; set; } = null!;

    }
}
