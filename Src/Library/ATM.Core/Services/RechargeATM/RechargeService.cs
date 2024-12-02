using ATM.Core.Data;
using ATM.Core.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Services.RechargeATM
{
    public class RechargeService : IRechargeService
    {
        private readonly IRepository<Domain.ATMs.RechargeATM> _rechargeRepository;
        private readonly IRepository<Domain.ATMs.ATM> _atmRepository;
        private readonly IRepository<Domain.ATMs.ATMWithdrawals> _atmWithdrawalsRepository;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="rechargeRepository"></param>
        /// <param name="atmRepository"></param>
        /// <param name="atmWithdrawalsRepository"></param>
        public RechargeService(IRepository<Domain.ATMs.RechargeATM> rechargeRepository, IRepository<Domain.ATMs.ATM> atmRepository, IRepository<Domain.ATMs.ATMWithdrawals> atmWithdrawalsRepository)
        {
            _rechargeRepository = rechargeRepository;
            _atmRepository = atmRepository;
            _atmWithdrawalsRepository = atmWithdrawalsRepository;

        }
        /// <summary>
        /// Gets ATM details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RechargeModel> GetATMDetails(Guid id)
        {
            var atmDetails = await _atmRepository.TableNoTracking
                .Include(x => x.RechargeATM)
                .Include(x => x.ATMWithdrawals)
                .Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            var model = new RechargeModel();
            if (atmDetails != null)
            {
                model.AtmId = id;
                model.Name = atmDetails.Name;
                model.Banknotes5000 = atmDetails.RechargeATM.Sum(x => x.Banknotes5000) - atmDetails.ATMWithdrawals.Sum(x => x.Banknotes5000);
                model.Banknotes2000 = atmDetails.RechargeATM.Sum(x => x.Banknotes2000) - atmDetails.ATMWithdrawals.Sum(x => x.Banknotes2000);
                model.Banknotes1000 = atmDetails.RechargeATM.Sum(x => x.Banknotes1000) - atmDetails.ATMWithdrawals.Sum(x => x.Banknotes1000);
                model.Banknotes500 = atmDetails.RechargeATM.Sum(x => x.Banknotes500) - atmDetails.ATMWithdrawals.Sum(x => x.Banknotes500);
                model.Total = (((int)model.Banknotes5000 * 5000) + ((int)model.Banknotes2000 * 2000) + ((int)model.Banknotes1000 * 1000) + ((int)model.Banknotes500 * 500));
            }
            return model;
        }

        /// <summary>
        /// Creates 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Result> CreateRecharge(RechargeModel model, Guid userId)
        {
            if (model == null)
            {
                return Result.Fail("Empty model");
            }
            var newBanknotes = new Domain.ATMs.RechargeATM()
            {
                AtmId = model.AtmId,
                Banknotes5000 = model.Banknotes5000,
                Banknotes2000 = model.Banknotes2000,
                Banknotes1000 = model.Banknotes1000,
                Banknotes500 = model.Banknotes500,
                CreatedById = userId,
                CreatedOnUtc = DateTime.Now,
                IsDeleted = false
            };

            try
            {
                await _rechargeRepository.InsertAsync(newBanknotes);
            }
            catch
            {
                return Result.Fail("An error occurred while trying to add this Banknotes. Maybe it already exists.");
            }
            return Result.Ok();

        }
    }
}
