using ATM.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Services.UsersAccount
{
    public interface IUserAccountService
    {
        /// <summary>
        /// Create User Account
        /// </summary>
        /// <param name="model"></param>
        /// <param name="createdById"></param>
        /// <returns></returns>
        Task<Result> AddUserAmount(UserAccountModel model, Guid createdById);

        /// <summary>
        /// Get user details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserAccountModel> GetUserAccount(Guid id);
    }
}
