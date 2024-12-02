using FluentValidation;

namespace ATM.Core.Services.Authentication
{
    /// <summary>
    /// Represents an authentication request.
    /// </summary>
    public class AuthenticationRequest
    {
        public AuthenticationRequest()
        {
            Username = string.Empty;
            Password = string.Empty;
            RememberMe = false;
        }
        public AuthenticationRequest(string username, string password, bool rememberMe = false)
        {
            Username = username;
            Password = password;
            RememberMe = rememberMe;
        }

        /// <summary>
        /// Gets or sets the username of the person requesting a login.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the person requesting a login.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a cookie should be set in the browser to remember the user.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// Validator for request.
        /// </summary>
        public class Validator : AbstractValidator<AuthenticationRequest>
        {
            public Validator()
            {
                // simple rules
                RuleFor(model => model.Username).NotEmpty().WithMessage("Ju lutem jepni email-in.");
                RuleFor(model => model.Password).NotEmpty().WithMessage("Ju lutem jepni fjalëkalimin.").MinimumLength(6).WithMessage("Fjalëkalimi duhet të përmbajë të paktën 6 karaktere.");
            }
        }
    }
}