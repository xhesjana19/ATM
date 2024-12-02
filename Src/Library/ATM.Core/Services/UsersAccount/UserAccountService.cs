using ATM.Core.Data;
using ATM.Core.Domain.Users;
using ATM.Core.Domain.UsersAccount;
using ATM.Core.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Services.UsersAccount
{
    /// <summary>
    /// Form managmentService
    /// </summary>
    public class UserAccountService : IUserAccountService
    {
        private readonly IRepository<UserAccount> _userAccountRepository;
        private readonly IRepository<User> _userRepository;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="userAccountRepository"></param>
        /// <param name="userRepository"></param>
        public UserAccountService(IRepository<UserAccount> userAccountRepository, IRepository<User> userRepository)
        {
            _userRepository = userRepository;
            _userAccountRepository = userAccountRepository;

        }

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserAccountModel> GetUserAccount(Guid id)
        {
            var userDetails = await _userRepository.GetByIdAsync(id);
            var model = new UserAccountModel();
            model.UserId = id;
            model.Name = userDetails.Name;
            return model;
        }

        /// <summary>
        /// Create 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="createdById"></param>
        /// <returns></returns>
        public async Task<Result>AddUserAmount(UserAccountModel model, Guid createdById)
        {
            if (model == null)
            {
                return Result.Fail("Empty model");
            }
            var userAmount = new UserAccount()
            {
                UserId = model.UserId,
                UserAmount = model.Amount,
                CreatedOnUtc = DateTime.Now,
                IsDeleted = false,
                CreatedById = createdById,
            };
            try
            {
                await _userAccountRepository.InsertAsync(userAmount);
            }
            catch
            {
                return Result.Fail("An error occurred while trying to add this Amount. Maybe it already exists.");
            }

            return Result.Ok();
        }
    }
}
