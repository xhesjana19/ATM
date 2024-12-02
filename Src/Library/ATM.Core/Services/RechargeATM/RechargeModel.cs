using ATM.Core.Services.ATMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Services.RechargeATM
{
    public class RechargeModel : BaseModel
    {
        public RechargeModel()
        {
            Name = string.Empty;
        }
        /// <summary>
        /// Gets and Sets Atm Name 
        /// </summary>
        public string Name { get; set; }

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

        /// <summary>
        /// Gets or sets the date this Atm was created on.
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
        /// Gets and Sets Total Amount in ATM
        /// </summary>

        public int? Total { get; set; }

    }
}
