using ATM.Core.Data;
using ATM.Core.Domain.Users;
using ATM.Core.Results;
using ATM.Core.Services.ATMs;
using ATM.Core.Services.RechargeATM;
using ATM.Core.Services.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Services.ATMWithdrawals
{
    /// <summary>
    /// Form managmentService
    /// </summary>
    public class ATMWithdrawalsService : IATMWithdrawalsService
    {
        private readonly IRepository<Domain.ATMs.ATMWithdrawals> _atmWithdrawalsRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Domain.ATMs.ATM> _atmRepository;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="atmWithdrawalsRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="atmRepository"></param>
        public ATMWithdrawalsService(IRepository<Domain.ATMs.ATMWithdrawals> atmWithdrawalsRepository, IRepository<User> userRepository, IRepository<Domain.ATMs.ATM> atmRepository)
        {
            _atmWithdrawalsRepository = atmWithdrawalsRepository;
            _userRepository = userRepository;
            _atmRepository = atmRepository;

        }

        /// <summary>
        ///  list of all account user details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IQueryable<ATMWithdrawalsModel>> GetAccountDetails(Guid userId)
        {
            var userDetails = _atmWithdrawalsRepository.TableNoTracking
              .Where(x => x.Users.Id == userId && x.AmountGet != 0)
              .Select(x => new ATMWithdrawalsModel()
              {
                  ATMName = x.Users.Name,
                  UserId = x.UserId,
                  Id = x.Id,
                  CreatedOnUtc = x.CreatedOnUtc,
                  AmountGet = x.AmountGet

              }).AsQueryable();
            return userDetails;
        }


        /// <summary>
        ///  list of all account user details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetTotalAmount(Guid userId)
        {
            var atmDetails = await _userRepository.TableNoTracking
                .Include(x => x.ATMWithdrawals)
                .Include(x => x.UsersAccount)
                .FirstOrDefaultAsync(x => x.Id == userId);

            int TotalAmount = (int)(atmDetails.UsersAccount.Sum(s => s.UserAmount) - atmDetails.ATMWithdrawals.Sum(s => s.AmountGet));
            return TotalAmount;
        }


        /// <summary>
        /// Gets all the atm for dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetATMList()
        {
            var atm = await _atmRepository.TableNoTracking
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .Select(ex => new SelectListItem
                {
                    Value = ex.Id.ToString(),
                    Text = ex.Name
                }).ToListAsync();

            return atm;
        }

        /// <summary>
        /// Creates a new ATM
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Result> CreateWithdraw(ATMWithdrawalsModel model, Guid userId)
        {
            //int amount = model.AmountSet;
            int sum = 0;
            if (model == null)
            {
                return Result.Fail("Empty model");
            }
            var banknotes = _atmRepository.TableNoTracking
                .Include(x => x.RechargeATM)
                .Include(x => x.ATMWithdrawals)
                .Where(x => x.Id == model.AtmId && !x.IsDeleted)
                .FirstOrDefault();
            var totalBanknotes = new RechargeModel()
            {
                Banknotes5000 = banknotes.RechargeATM.Sum(x => x.Banknotes5000) - banknotes.ATMWithdrawals.Sum(x => x.Banknotes5000),
                Banknotes2000 = banknotes.RechargeATM.Sum(x => x.Banknotes2000) - banknotes.ATMWithdrawals.Sum(x => x.Banknotes2000),
                Banknotes1000 = banknotes.RechargeATM.Sum(x => x.Banknotes1000) - banknotes.ATMWithdrawals.Sum(x => x.Banknotes1000),
                Banknotes500 = banknotes.RechargeATM.Sum(x => x.Banknotes500) - banknotes.ATMWithdrawals.Sum(x => x.Banknotes500),
            };

            int c5000 = 0;
            int c2000 = 0;
            int c1000 = 0;
            int c500 = 0;

            var banknotesList = new List<int>() { 5000, 2000, 1000, 500 };
            foreach (var item in banknotesList)
            {
                switch (item)
                {
                    case 5000:
                        if (totalBanknotes.Banknotes5000 == 0)
                            continue;
                        if (model.AmountSet >= item)
                        {
                            sum = model.AmountSet / item;
                            if (totalBanknotes.Banknotes5000 >= sum)
                            {
                                totalBanknotes.Banknotes5000 -= sum;
                                model.AmountSet = model.AmountSet - (sum * 5000);
                            }
                            if (totalBanknotes.Banknotes5000 < sum && totalBanknotes.Banknotes5000 != 0)
                            {
                                sum = (int)totalBanknotes.Banknotes5000;
                                model.AmountSet = (int)(totalBanknotes.Banknotes1000 * 5000);
                            }
                            c5000 = sum;
                            sum = 0;
                        }
                        break;
                    case 2000:
                        if (totalBanknotes.Banknotes2000 == 0)
                            continue;
                        if (model.AmountSet >= item)
                        {
                            sum = model.AmountSet / item;
                            if (totalBanknotes.Banknotes2000 >= sum)
                            {
                                totalBanknotes.Banknotes2000 -= sum;
                                model.AmountSet = model.AmountSet - (sum * 2000);
                            }
                            if (totalBanknotes.Banknotes2000 < sum && totalBanknotes.Banknotes2000 != 0)
                            {
                                sum = (int)totalBanknotes.Banknotes2000;
                                model.AmountSet = (int)(totalBanknotes.Banknotes1000 * 2000);
                            }
                            c2000 = sum;
                            sum = 0;
                        }
                        break;
                    case 1000:
                        if (totalBanknotes.Banknotes1000 == 0)
                            continue;
                        if (model.AmountSet >= item)
                        {
                            sum = model.AmountSet / item;
                            if (totalBanknotes.Banknotes1000 >= sum)
                            {
                                totalBanknotes.Banknotes1000 -= sum;
                                model.AmountSet = model.AmountSet - (sum * 1000);                            
                            }
                            if (totalBanknotes.Banknotes1000 < sum && totalBanknotes.Banknotes1000 != 0)
                            {
                                sum = (int)totalBanknotes.Banknotes1000;
                                model.AmountSet = (int)(totalBanknotes.Banknotes1000 * 1000);
                            }
                            c1000 = sum;
                            sum = 0;
                        }
                        break;
                    case 500:
                        if (totalBanknotes.Banknotes500 == 0)
                            continue;
                        if (model.AmountSet >= item)
                        {
                            sum = model.AmountSet / item;
                            if (totalBanknotes.Banknotes500 >= sum) 
                            {
                                totalBanknotes.Banknotes500 -= sum;
                                model.AmountSet = model.AmountSet - (sum * 500);                               
                            }
                            if (totalBanknotes.Banknotes500 < sum && totalBanknotes.Banknotes500 != 0)
                            {
                                sum = (int)totalBanknotes.Banknotes500;
                                model.AmountSet = (int)(totalBanknotes.Banknotes500 * 500);
                            }
                            c500 = sum;
                            sum = 0;
                        }
                        break;
                }
            }

            var newATMWithdrawals = new Domain.ATMs.ATMWithdrawals()
            {
                Banknotes5000 = c5000 != 0 ? c5000 : null,
                Banknotes2000 = c2000 != 0 ? c2000 : null,
                Banknotes1000 = c1000 != 0 ? c1000 : null,
                Banknotes500 = c500 != 0 ? c500 : null,
                AmountSet = model.Amount,
                AmountGet = model.AmountSet == ((c5000 * 5000) + (c2000 * 2000) + (c1000 * 1000) + (c500 * 500)) ? model.AmountSet : 0,
                UserId = userId,
                AtmId = model.AtmId,
                CreatedById = userId,
                CreatedOnUtc = DateTime.Now,
                IsDeleted = false
            };

            try
            {
                await _atmWithdrawalsRepository.InsertAsync(newATMWithdrawals);
            }
            catch
            {
                return Result.Fail("An error occurred while trying to add this Withdraw. Maybe it already exists.");
            }
            return Result.Ok();

        }

        /// <summary>
        /// Get all ATMs from the database
        /// </summary>
        public async Task<IQueryable<ATMWithdrawalsModel>> GetReportDetails()
        {
            var atmList = _atmWithdrawalsRepository.TableNoTracking.AsQueryable()
                .Include(x=> x.ATM)
                .Where(x => !x.IsDeleted && x.AmountGet != 0)
                .Select(x => new ATMWithdrawalsModel()
                {
                    Id = x.Id,
                    ATMName = x.ATM.Name,
                    Banknotes5000 = x.Banknotes5000,
                    Banknotes2000 = x.Banknotes2000,
                    Banknotes1000 = x.Banknotes1000,
                    Banknotes500 = x.Banknotes500,
                    CreatedOnUtc = x.CreatedOnUtc
                });
            return atmList;
        }
    }
}

