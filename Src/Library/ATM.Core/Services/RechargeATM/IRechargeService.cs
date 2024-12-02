using ATM.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Services.RechargeATM
{

    /// <summary>
    /// Form managmentService
    /// </summary>
    public interface IRechargeService
    {
        /// <summary>
        /// Gets ATM details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RechargeModel> GetATMDetails(Guid id);

        /// <summary>
        /// Creates 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result> CreateRecharge(RechargeModel model, Guid userId);
    }
}
