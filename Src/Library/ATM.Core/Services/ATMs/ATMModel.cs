using ATM.Core.Data;
using ATM.Core.Services.ATMWithdrawals;
using ATM.Core.Services.RechargeATM;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATM.Core.Services.ATMs
{
    /// <summary>
    /// Create ATM request
    /// </summary>
    public class ATMModel : BaseModel
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public ATMModel()
        {
            Name = string.Empty;
        }
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
                return this.CreatedOnUtc.ToString("dd/MM/yyyy"); ;
            }
        }

        /// <summary>
        /// Gets or sets the name 
        /// </summary> 
        [Display(Name = "Emri")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the Total Amount in ATM
        /// </summary>
        public int? TotalAmount { get; set; }

        public List<ATMWithdrawalsModel> ATMWithdrawalsList { get; set; } = new List<ATMWithdrawalsModel>();
        public List<RechargeModel> RechargeModelList { get; set; } = new List<RechargeModel>();
    }
    /// <summary>
    /// Validator class.
    /// </summary>
    public class AtmModelValidator : AbstractValidator<ATMModel>
    {
        private readonly IRepository<Domain.ATMs.ATM> _atmRepository;

        public AtmModelValidator(IRepository<Domain.ATMs.ATM> atmRepository)
        {
            _atmRepository = atmRepository;

            // simple rules
            RuleFor(model => model.Name)
                .NotEmpty()
                .NotNull()
                .WithName("Name");

            // advanced
            RuleFor(model => model).CustomAsync(ValidateFormDoesNotExist);

        }

        /// <summary>
        /// Valid Exist ATM Name
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ValidateFormDoesNotExist(ATMModel model, CustomContext context, CancellationToken cancellationToken)
        {
            var AtmName = await _atmRepository.TableNoTracking
                .Where(u => u.Name == model.Name && u.Id != model.Id && !u.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
            if (AtmName != null)
            {
                context.AddFailure(nameof(model.Name), "An ATM with this name exists.");
            }
        }

    }
}
