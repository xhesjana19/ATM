using ATM.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Domain.ATMs
{
    public class ATMWithdrawals : AuditableEntity
    {
        /// <summary>
        /// Gets and sets Atm Id
        /// </summary>
        public Guid AtmId { get; set; }
        public virtual ATM ATM { get; set; } = null!;

        /// <summary>
        /// Gets and Sets User Id
        /// </summary>
        public Guid UserId { get; set; }
        public virtual User Users { get; set; } = null!;

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
        /// Gets and Sets the amount placed in atm
        /// </summary>
        public int AmountSet { get; set; }

        /// <summary>
        /// Gets and Sets the amount taken from atm
        /// </summary>
        public int? AmountGet { get; set; }

    }
}
