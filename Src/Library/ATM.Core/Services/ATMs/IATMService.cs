using ATM.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATM.Core.Services.ATMs
{
    public interface IATMService
    {
        /// <summary>
        /// return list of all ATM
        /// </summary>
        Task<IQueryable<ATMModel>> GetAllATMs();

        /// <summary>
        /// Creates a new ATM
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result> CreateATM(ATMModel model, Guid userId);

        /// <summary>
        /// Gets ATM details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ATMModel> GetATMDetailsAsync(Guid id);

        /// <summary>
        /// Edit an ATM
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result> EditAtm(ATMModel model, Guid userId, CancellationToken cancellation);

        /// <summary>
        /// Delete inATMdustry
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result> DeleteATM(Guid id, CancellationToken cancellation);

        /// <summary>
        /// return list of amount <= 5000 ALL
        /// </summary>
        Task<IQueryable<ATMModel>> GetFilterAmount();
    }
}
