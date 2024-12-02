using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Services.UsersAccount
{
    /// <summary>
    /// Create Discount Model
    /// </summary>
    public class UserAccountModel : BaseModel
    {
        public UserAccountModel()
        {
            Name = string.Empty;
        }

        /// <summary>
        /// Gests or sets the Name
        /// </summary>
        [Display(Name = "Name")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets User Amount
        /// </summary>
        [Display(Name = "User Amount")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets and Sets User Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Total Amount
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
