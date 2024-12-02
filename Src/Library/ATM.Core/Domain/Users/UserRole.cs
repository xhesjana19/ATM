using System.ComponentModel.DataAnnotations;

namespace ATM.Core.Domain.Users
{
    public enum UserRole
    {
        /// <summary>
        /// Administrator.
        /// </summary>
        [Display(Name = "Administrator")]
        Administrator = 1,
        /// <summary>
        /// Editor.
        /// </summary>
        [Display(Name = "User")]
        User = 2,

    }
}
