using ATM.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using ATM.Core.Services.ATMWithdrawals;

namespace ATM.Core.Services.Users
{
    /// <summary>
    /// User model (main).
    /// </summary>
    public class UserModel : BaseModel
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public UserModel()
        {
            Name = string.Empty;
            Username = string.Empty;
            Password = string.Empty;

        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a new password.
        /// </summary>
        public bool GenerateNewPassword { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public UserRole Role { get; set; }
        public string RoleName { get; set; }
        public bool Active { get; set; }

        #region ATM properties
        /// <summary>
        /// Gets and Sets User Amount
        /// </summary>
        public int TotalAmount { get; set; }

        /// <summary>
        /// Gets and sets ammount withdrawal date
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// CreatedOnUtc column converted to date time formatted
        /// </summary>
        public string CreatedOnDateTime
        {
            get
            {
                return this.CreatedOnUtc.ToString("dd/MM/yyyy"); 
            }
        }

        /// <summary>
        /// Gets and Sets the amount taken from atm
        /// </summary>
        public int? AmountGet { get; set; }

        /// <summary>
        /// Gets and Sets Atm Id
        /// </summary>
        public Guid AtmId { get; set; }
        public List<ATMWithdrawalsModel> ATMWithdrawalsList { get; set; }  = new List<ATMWithdrawalsModel>();
        #endregion
    }


}
