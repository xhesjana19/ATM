using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ATM.Core.Data;
using ATM.Core.Domain.Users;
using ATM.Core.Results;
using ATM.Core.Services.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using ATM.Core.Domain.UsersAccount;
using ATM.Core.Services.Authentication;

namespace ATM.Core.Services.Users
{
    /// <summary>
    /// User management service.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IEncryptionService _encryptionService;


        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="encryptionService"></param>
        public UserService(IRepository<User> userRepository, IMapper mapper, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _encryptionService = encryptionService;
        }

        /// <summary>
        /// Gets a queryable of users without password column.
        /// </summary>
        public IQueryable<UserModel> GetUsersWithoutPassword()
        {
            var query = _userRepository.Table
                .Where(x => !x.IsDeleted)
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Username = u.Username,
                    Password = "",
                    RoleName = GetDisplayName(u.Role),
                    Active = u.IsActive

                });

            return query;
        }

        /// <summary>
        /// Gets a queryable of users of the given ids from the database.
        /// </summary>
        public IQueryable<UserModel> GetUsersByIds(IEnumerable<Guid> userIds)
        {
            var query = _userRepository.TableNoTracking
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Username = u.Username,
                    Password = "",
                    Role = u.Role
                });
            return query;
        }

        /// <summary>
        /// Gets a queryable of users of the specified role.
        /// </summary>
        public IQueryable<UserModel> GetUsersByRole(UserRole userRole)
        {
            var query = _userRepository.TableNoTracking
                .Where(u => u.Role == userRole)
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Username = u.Username,
                    Password = "",
                    Role = u.Role
                });

            return query;
        }

        /// <summary>
        /// Gets a user from database.
        /// </summary>
        public async Task<UserModel> GetUserByIdAsync(string userId)
        {
            var user = await _userRepository.TableNoTracking
                .Where(u => u.Id.ToString() == userId)
                .FirstOrDefaultAsync();

            return new UserModel()
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Password = user.Password,
                Role = user.Role
            };
        }

        /// <summary>
        /// Creates a new user and inserts it on the database.
        /// </summary>
        public async Task<Result> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<User>(request);

            entity.PasswordCipher = request.Password;
            entity.Password = _encryptionService.CreatePasswordHash(request.Password);
            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.CreatedById = request.CreatedBy;
            entity.CreatedOnUtc = DateTime.Now;
            entity.Username = request.Username.Trim();

            await _userRepository.InsertAsync(entity, cancellationToken);

            return Result.Ok();
        }

        /// <summary>
        /// Updates a user in the database.
        /// </summary>
        public async Task<Result> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await _userRepository.TableNoTracking.FirstOrDefaultAsync(u => u.Id == request.Id);
            request.CurrentRole = entity.Role; // Get the role before updating
            entity.Name = request.Name;
            entity.Username = request.Username;
            entity.Role = request.Role;
          
            entity.UpdatedById = request.UpdatedBy;

            if (!string.IsNullOrEmpty(request.Password))
            {
                entity.PasswordCipher = request.Password;
                entity.Password = _encryptionService.CreatePasswordHash(request.Password);             
            }
            await _userRepository.UpdateAsync(entity, cancellationToken);
            return Result.Ok();
        }

        /// <summary>
        /// Edits the user credentials.
        /// </summary>
        public async Task<Result> EditUserCredentialsAsync(EditUserCredentialsRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await _userRepository.TableNoTracking.FirstOrDefaultAsync(u => u.Id == request.Id);
            entity.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
                entity.Password = _encryptionService.CreatePasswordHash(request.NewPassword);

            await _userRepository.UpdateAsync(entity, cancellationToken);

            return Result.Ok();
        }

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        public async Task<Result> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = await _userRepository.TableNoTracking.FirstOrDefaultAsync(u => u.Id == userId);
            entity.IsDeleted = true;

            await _userRepository.UpdateAsync(entity, cancellationToken);

            return Result.Ok();
        }



        #region Hidden

        /// <summary>
        /// Generates a new password.
        /// </summary>
        private string GenerateNewPassword(int length, bool useLowercase, bool useUppercase, bool useNumbers, bool useSpecial)
        {
            const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
            const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMBERS = "0123456789";
            const string SPECIALS = @"!@£$%^&*()#€";

            char[] password = new char[length];
            string charSet = ""; // Initialise to blank
            Random random = new Random();

            // Build up the character set to choose from
            if (useLowercase) charSet += LOWER_CASE;
            if (useUppercase) charSet += UPPER_CAES;
            if (useNumbers) charSet += NUMBERS;
            if (useSpecial) charSet += SPECIALS;

            for (int i = 0; i < length; i++)
                password[i] = charSet[random.Next(charSet.Length - 1)];

            return string.Join(null, password);
        }

        #endregion

        private static string GetDisplayName(UserRole value)
        {
            var attribute = (DisplayNameAttribute)value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(false)
                .Where(a => a is DisplayNameAttribute)
                .FirstOrDefault();

            return attribute != null ? attribute.DisplayName : value.ToString();
        }
    }
}
