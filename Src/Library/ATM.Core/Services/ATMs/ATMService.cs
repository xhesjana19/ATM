using ATM.Core.Data;
using ATM.Core.Results;
using ATM.Core.Services.RechargeATM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATM.Core.Services.ATMs
{
    /// <summary>
    /// Form managmentService
    /// </summary>
    public class ATMService : IATMService
    {
        private readonly IRepository<Domain.ATMs.ATM> _atmRepository;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="AtmRepository"></param>
        public ATMService(IRepository<Domain.ATMs.ATM> atmRepository)
        {
            _atmRepository = atmRepository;

        }

        /// <summary>
        /// Get all ATMs from the database
        /// </summary>
        public async Task<IQueryable<ATMModel>> GetAllATMs()
        {
            var atmList = _atmRepository.TableNoTracking.AsQueryable()
                .Where(x => !x.IsDeleted)
                .Select(x => new ATMModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedOnUtc = x.CreatedOnUtc
                });
            return atmList;
        }

        /// <summary>
        /// Creates a new ATM
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Result> CreateATM(ATMModel model, Guid userId)
        {
            if (model == null)
            {
                return Result.Fail("Empty model");
            }
            var newATM = new Domain.ATMs.ATM()
            {
                Name = model.Name,
                CreatedById = userId,
                CreatedOnUtc = DateTime.Now,
                IsDeleted = false
            };

            try
            {
                await _atmRepository.InsertAsync(newATM);
            }
            catch
            {
                return Result.Fail("An error occurred while trying to add this ATM. Maybe it already exists.");
            }
            return Result.Ok();

        }

        /// <summary>
        /// Gets ATM details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ATMModel> GetATMDetailsAsync(Guid id)
        {
            var atmDetails = await _atmRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == id);
            var model = new ATMModel();
            model.Id = id;
            model.Name = atmDetails.Name;
            return model;

        }

        /// <summary>
        /// Updates Atm details
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<Result> EditAtm(ATMModel model, Guid userId, CancellationToken cancellation)
        {
            try
            {
                var editAtm = await _atmRepository.GetByIdAsync(model.Id);

                if (editAtm == null)
                    return Result.Fail("ATM with this Id was not found");

                editAtm.Name = model.Name;
                editAtm.UpdatedById = userId;
                editAtm.UpdatedOnUtc = DateTime.Now;

                await _atmRepository.UpdateAsync(editAtm, cancellation);
            }
            catch
            {
                return Result.Fail("An error occurred while trying to delete this ATM");
            }
            return Result.Ok();
        }

        /// <summary>
        /// Delete an ATM
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<Result> DeleteATM(Guid id, CancellationToken cancellation)
        {
            try
            {
                //soft deleted
                var deleteAtm = await _atmRepository.GetByIdAsync(id, cancellation);

                if (deleteAtm == null)
                    return Result.Fail("ATM with this Id was not found");
                deleteAtm.IsDeleted = true;

                await _atmRepository.UpdateAsync(deleteAtm, cancellation);
            }
            catch
            {
                return Result.Fail("An error occurred while trying to delete this ATM");
            }
            return Result.Ok();
        }

        /// <summary>
        /// Get all ATMs from the database
        /// </summary>
        public async Task<IQueryable<ATMModel>> GetFilterAmount()
        {
            var atmDetails = _atmRepository.TableNoTracking
                        .AsQueryable()
                        .Include(x => x.RechargeATM)
                        .Include(x => x.ATMWithdrawals)
                        .Where(x => !x.IsDeleted)
                        .Select(x => new ATMModel()
                        {
                            CreatedOnUtc = x.CreatedOnUtc,
                            Name = x.Name,
                            Id = x.Id,
                            TotalAmount = ((x.RechargeATM.Sum(s => s.Banknotes5000) - x.ATMWithdrawals.Sum(s => s.Banknotes5000)) * 5000) +
                                          ((x.RechargeATM.Sum(s => s.Banknotes2000) - x.ATMWithdrawals.Sum(s => s.Banknotes2000)) * 2000) +
                                          ((x.RechargeATM.Sum(s => s.Banknotes1000) - x.ATMWithdrawals.Sum(s => s.Banknotes1000)) * 1000) +
                                          ((x.RechargeATM.Sum(s => s.Banknotes500) - x.ATMWithdrawals.Sum(s => s.Banknotes500)) * 500),
                        }).Where(x => x.TotalAmount <= 5000);

            return atmDetails;
        }

    }
}


