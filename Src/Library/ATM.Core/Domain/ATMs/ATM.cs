using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Domain.ATMs
{
    public class ATM : AuditableEntity
    {
        public ATM()
        {
            Name = string.Empty;
        }
        /// <summary>
        /// Gets and ATM Name
        /// </summary>
        public string Name { get; set; }

        public virtual ICollection<RechargeATM> RechargeATM { get; set; } = null!;
        public virtual ICollection<ATMWithdrawals> ATMWithdrawals { get; set; } = null!;
    }
}
