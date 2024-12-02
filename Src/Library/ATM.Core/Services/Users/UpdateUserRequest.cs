using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using ATM.Core.Data;
using ATM.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;


namespace ATM.Core.Services.Users
{
    /// <summary>
    /// Update user request.
    /// </summary>
    public class UpdateUserRequest : BaseModel
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public UpdateUserRequest()
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
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets a new password.
        /// </summary>
        public string? ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        [Display(Name = "Roli")]
        //[Required(ErrorMessage = ErrorMessages.Required)]
        public UserRole Role { get; set; }

        /// <summary>
        /// Gets or sets previous role.
        /// </summary>
        public UserRole CurrentRole { get; set; }
        public Guid UpdatedBy { get; set; }

        /// <summary>
        /// Validator for request.
        /// </summary>
        public class Validator : AbstractValidator<UpdateUserRequest>
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

                RuleFor(model => model.Username)
                    .NotNull()
                    .NotEmpty()
                    .EmailAddress();

                RuleFor(model => model.Role)
                    .NotNull()
                    .NotEmpty();

                //RuleFor(model => model.Password)
                //   .NotEmpty();

                //RuleFor(model => model.ConfirmPassword)
                //    .NotEmpty();

                RuleFor(model => model.ConfirmPassword)
                    .Equal(m => m.Password)
                    .WithMessage("Password and Confirm Password should be equal to each other.");

                // advanced
                RuleFor(model => model).CustomAsync(ValidateUserDoesNotExist);

            }

            private async Task ValidateUserDoesNotExist(UpdateUserRequest model, CustomContext context, CancellationToken cancellationToken)
            {
                var user = await _userRepository.TableNoTracking
                    .Where(u => u.Username == model.Username && u.Id != model.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (user != null)
                    context.AddFailure(nameof(model.Username), "Një perdorues me kete email ekziston.");
            }


        }
    }
}
