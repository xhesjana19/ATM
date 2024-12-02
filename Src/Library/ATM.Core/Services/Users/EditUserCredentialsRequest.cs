using FluentValidation;
using FluentValidation.Validators;
using ATM.Core.Data;
using ATM.Core.Domain.Users;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ATM.Core;

namespace ATM.Core.Services.Users
{
    /// <summary>
    /// Edit user credentials request.
    /// </summary>
    public class EditUserCredentialsRequest : BaseModel
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public EditUserCredentialsRequest()
        {
            Name = string.Empty;
            Username = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [Display(Name = "Emri")]
        //[Required(ErrorMessage = ErrorMessages.Required)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Display(Name = "Fjalëkalimi i ri")]
        public string? NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Display(Name = "Konfirmo Fjalëkalimin")]
        public string? ConfirmPassword { get; set; }

        /// <summary>
        /// Validator for request.
        /// </summary>
        public class Validator : AbstractValidator<EditUserCredentialsRequest>
        {
            private readonly IRepository<User> _userRepository;

            public Validator(IRepository<User> userRepository)
            {
                _userRepository = userRepository;

                // simple rules
                RuleFor(model => model.Name)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(3)
                    .MaximumLength(20);

                RuleFor(model => model.NewPassword)
                    .MinimumLength(6)
                    .WithMessage("Fjalëkalimi duhet të përmbajë të paktën 6 karaktere.");

                // advanced
                RuleFor(model => model).CustomAsync(ValidateUserDoesNotExist);
                RuleFor(model => model).Custom(ValidatePasswordMatch);
            }

            private async Task ValidateUserDoesNotExist(EditUserCredentialsRequest model, CustomContext context, CancellationToken cancellationToken)
            {
                var user = await _userRepository.TableNoTracking
                    .Where(u => u.Username == model.Username && u.Id != model.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (user != null)
                    context.AddFailure(nameof(model.Username), "Një perdorues me kete email ekziston.");
            }

            private void ValidatePasswordMatch(EditUserCredentialsRequest model, CustomContext context)
            {
                if (model.NewPassword != model.ConfirmPassword)
                    context.AddFailure(nameof(model.ConfirmPassword), "Fjalëkalimet nuk përputhen.");
            }
        }
    }
}
