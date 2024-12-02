using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Services.Users
{
    /// <summary>
    /// Delete user request.
    /// </summary>
    public class DeleteUserRequest : BaseModel
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public DeleteUserRequest()
        {
            Name = string.Empty;
            Username = string.Empty;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Validator for request.
        /// </summary>
        public class Validator : AbstractValidator<DeleteUserRequest>
        {
            public Validator()
            {
                // simple rules
                RuleFor(model => model.Id).NotEmpty().NotNull();
            }
        }
    }
}
