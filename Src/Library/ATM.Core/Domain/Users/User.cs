using ATM.Core.Domain.ATMs;
using ATM.Core.Domain.UsersAccount;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATM.Core.Domain.Users
{
    /// <summary>
    /// User entity.
    /// </summary>
    public class User : AuditableEntity
    {
        public User()
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
        /// Gets or sets the password cipher.
        /// </summary>
        public string PasswordCipher { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public UserRole Role { get; set; }


        /// <summary>
        /// Gets or sets IsActive value
        /// </summary>
        public bool IsActive { get; set; }

        public virtual ICollection<UserAccount> UsersAccount { get; set; } = null!;
        public virtual ICollection<ATMWithdrawals> ATMWithdrawals { get; set; } = null!;

        [NotMapped]
        public string RoleString
        {
            get
            {
                var roleString = string.Empty;
                switch (Role)
                {
                    case UserRole.Administrator:
                        roleString = "Administrator";
                        break;
                    case UserRole.User:
                        roleString = "User";
                        break;
                }
                return roleString;
            }
        }
    }
}
