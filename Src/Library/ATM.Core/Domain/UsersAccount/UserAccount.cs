using ATM.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Domain.UsersAccount
{
    /// <summary>
    /// UserAccount entity. 
    /// </summary>
    public class UserAccount: AuditableEntity
    {
        /// <summary>
        /// Gets and Sets User Amount
        /// </summary>
        public decimal UserAmount { get; set; }

        /// <summary>
        /// Gets or sets the user
        /// </summary>
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
