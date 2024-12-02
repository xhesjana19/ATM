using ATM.Core.Domain.Users;
using ATM.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATM.Core.Services.Users
{
    /// <summary>
    /// User management service.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets a queryable of users without password column.
        /// </summary>
        IQueryable<UserModel> GetUsersWithoutPassword();

        /// <summary>
        /// Gets a queryable of users of the given ids from the database.
        /// </summary>
        IQueryable<UserModel> GetUsersByIds(IEnumerable<Guid> userIds);

        /// <summary>
        /// Gets a queryable of users of the specified role.
        /// </summary>
        IQueryable<UserModel> GetUsersByRole(UserRole userRole);

        /// <summary>
        /// Gets a user from database.
        /// </summary>
        Task<UserModel> GetUserByIdAsync(string userId);

        /// <summary>
        /// Creates a new user and inserts it on the database.
        /// </summary>
        Task<Result> CreateUserAsync(CreateUserRequest user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a user in the database.
        /// </summary>
        Task<Result> UpdateUserAsync(UpdateUserRequest user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Edits the user credentials.
        /// </summary>
        Task<Result> EditUserCredentialsAsync(EditUserCredentialsRequest user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        Task<Result> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
      
    }
}
