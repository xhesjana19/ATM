using ATM.Core.Data;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Services.ATMWithdrawals
{
    public class ATMWithdrawalsModel : BaseModel
    {

        /// <summary>
        /// Gets and Sets User Id
        /// </summary>
        public Guid UserId { get; set; }

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

        /// <summary>
        /// Gets and sets Amount
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Gets and sets Total Amount
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
        /// Gets and sets ATM name 
        /// </summary>
        public string? ATMName { get; set; } = null;

        /// <summary>
        /// Gets and setsATM  id
        /// </summary>
        [Display(Name = "Name")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public Guid AtmId { get; set; }

        /// <summary>
        /// Gets or sets list of all ATM
        /// </summary>
        public List<SelectListItem> AllATMList { get; set; } = new List<SelectListItem>();
    }
    /// <summary>
    /// Validator class.
    /// </summary>
    public class AtmModelValidator : AbstractValidator<ATMWithdrawalsModel>
    {
        private readonly IRepository<ATM.Core.Domain.ATMs.ATMWithdrawals> _atmWithdrawalsRepository;

        public AtmModelValidator(IRepository<ATM.Core.Domain.ATMs.ATMWithdrawals> atmWithdrawalsRepository)
        {
            _atmWithdrawalsRepository = atmWithdrawalsRepository;

            // simple rules
            RuleFor(model => model.AtmId)
                .NotEmpty()
                .WithName("ATM");
        }

    }
}
