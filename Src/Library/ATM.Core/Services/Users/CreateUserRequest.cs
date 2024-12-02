using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using ATM.Core;
using ATM.Core.Data;
using ATM.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATM.Core.Services.Users
{
    /// <summary>
    /// Create user request.
    /// </summary>
    public class CreateUserRequest : BaseModel
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public CreateUserRequest()
        {
            Name = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
 
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
        [Display(Name = "Email")]
        //[Required(ErrorMessage = ErrorMessages.Required)]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        [Display(Name = "Roli")]
        //[Required(ErrorMessage = ErrorMessages.Required)]
        public UserRole Role { get; set; }


        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Validator for request.
        /// </summary>
        public class Validator : AbstractValidator<CreateUserRequest>
        {
            private readonly IRepository<User> _userRepository;

            public Validator(IRepository<User> userRepository)
            {
                _userRepository = userRepository;

                // simple rules
                RuleFor(model => model.Name)
                    .NotEmpty()
                    .NotNull()
                    .MinimumLength(3)
                    .MaximumLength(20)
                    .WithMessage("Fusha '{PropertyName}' është e detyrueshme.");

                RuleFor(model => model.Username)
                    .NotEmpty()
                    .NotNull()
                    .EmailAddress()
                    .WithMessage("Fusha '{PropertyName}' është e detyrueshme.");

                RuleFor(model => model.Role)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Fusha '{PropertyName}' është e detyrueshme.");

                RuleFor(model => model.Password)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Fusha '{PropertyName}' është e detyrueshme.");

                RuleFor(model => model.ConfirmPassword)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Fusha '{PropertyName}' është e detyrueshme.");

                RuleFor(model => model.ConfirmPassword)
                    .NotNull()
                    .NotEmpty()
                    .Equal(m => m.Password)
                    .WithMessage("Password dhe Konfirmo Password duhet të jenë të njëjtë");

                // advanced
                RuleFor(model => model).CustomAsync(ValidateUserDoesNotExist);
      
            }

            private async Task ValidateUserDoesNotExist(CreateUserRequest model, CustomContext context, CancellationToken cancellationToken)
            {
                var user = await _userRepository.TableNoTracking
                    .Where(u => u.Username == model.Username)
                    .FirstOrDefaultAsync(cancellationToken);

                if (user != null)
                    context.AddFailure(nameof(model.Username), "Një përdorues me këtë email ekziston.");
            }

      

            
        }
    }
}
