using ATM.Core.Domain.Users;

namespace ATM.Core.Services.Authentication
{
    /// <summary>
    /// Represents an authenticated user.
    /// </summary>
    public class AuthenticatedUser : BaseModel
    {
        public AuthenticatedUser()
        {
            Name = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        //public UserRole Role { get; set; }

        public UserRole Role { get; set; }
    }
}