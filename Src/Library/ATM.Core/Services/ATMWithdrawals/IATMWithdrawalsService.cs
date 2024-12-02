using ATM.Core.Results;
using ATM.Core.Services.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Services.ATMWithdrawals
{
    /// <summary>
    /// return list of all ATM
    /// </summary>
    public interface IATMWithdrawalsService
    {
        /// <summary>
        ///  return list of all account user details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IQueryable<ATMWithdrawalsModel>> GetAccountDetails(Guid userId);

        /// <summary>
        /// return Total Amount
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetTotalAmount(Guid userId);

        /// <summary>
        /// Return atm select list 
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItem>> GetATMList();

        /// <summary>
        /// Create new Withdraw
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result> CreateWithdraw(ATMWithdrawalsModel model, Guid userId);

        /// <summary>
        /// return list of all ATM
        /// </summary>
        Task<IQueryable<ATMWithdrawalsModel>> GetReportDetails();
    }
}
