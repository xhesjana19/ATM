using ATM.Core;
using ATM.Core.Services.ATMWithdrawals;
using ATM.Core.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Web.Controllers
{
    public class ATMWithdrawalsController : Controller
    {
        #region Ctor.
        /// <summary>
        /// Ctor.
        /// </summary>
        private readonly IATMWithdrawalsService _atmWithdrawalsService;
        private readonly Core.Services.Authentication.IAuthenticationService _authenticationService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="atmWithdrawalsService"></param>
        /// <param name="authenticationService"></param>
        public ATMWithdrawalsController(IATMWithdrawalsService atmWithdrawalsService, Core.Services.Authentication.IAuthenticationService authenticationService)
        {
            _atmWithdrawalsService = atmWithdrawalsService;
            _authenticationService = authenticationService;
        }
        #endregion

        #region List && Details
        [HttpGet]
        [Authorize(Roles = Roles.User)]
        public async Task<IActionResult> Index(ATMWithdrawalsModel model)
        {
            // check for permissions
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            if (user.Result.Role.Equals("User"))
            {
                return RedirectToAction("Denied", "Error");
            }
             model.TotalAmount = await _atmWithdrawalsService.GetTotalAmount(user.Result.Id);
             //return View();
            return View("~/Views/ATMWithdrawals/Index.cshtml", new ATMWithdrawalsModel() { UserId = user.Result.Id, TotalAmount= model.TotalAmount});
        }

        /// <summary>
        /// Display List of ATM
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task<ActionResult> AccountData(Guid userId)
        {
            var atm = await _atmWithdrawalsService.GetAccountDetails(userId);
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                switch (sortColumn)
                {
                    case "Name":
                        {
                            atm = sortColumnDirection == "desc" ? atm.OrderByDescending(x => x.ATMName) : atm.OrderBy(x => x.ATMName);
                        }

                        break;
                    case "Amount":
                        {
                            atm = sortColumnDirection == "desc" ? atm.OrderByDescending(x => x.AmountGet) : atm.OrderBy(x => x.AmountGet);
                        }

                        break;
                    case "Date":
                        {
                            atm = sortColumnDirection == "desc" ? atm.OrderByDescending(x => x.CreatedOnUtc) : atm.OrderBy(x => x.CreatedOnUtc);
                        }
                        break;
                }
            }
            else
            {
                atm = atm.OrderByDescending(x => x.CreatedOnUtc);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                atm = atm.Where(m => (m.ATMName != null && m.ATMName.Contains(searchValue))
                                                     || (m.CreatedOnUtc.ToString() != null && m.CreatedOnUtc.ToString().Contains(searchValue)));
            }

            recordsTotal = atm.Count();
            var data =await  atm.Skip(skip).Take(pageSize).ToListAsync();
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
        }
        #endregion

        #region CRUD Operation
        /// <summary>
        /// Create an ATM
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Roles.User)]
        public async Task<IActionResult> Create()
        {
            // check for permissions
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            if (user.Result.Role.Equals("Administrator"))
            {
                return RedirectToAction("Denied", "Error");
            }

            var model = new ATMWithdrawalsModel();
            model.AllATMList = await _atmWithdrawalsService.GetATMList();
            model.UserId = user.Result.Id;
            return View("~/Views/ATMWithdrawals/Create.cshtml", model);
        }

        /// <summary>
        /// Create an ATM
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task<IActionResult> Create(ATMWithdrawalsModel model)
        {
            // check for permissions
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            if (user.Result.Role.Equals("Administrator"))
            {
                return RedirectToAction("Denied", "Error");
            }

            if (!ModelState.IsValid)
            {
                model.AllATMList = await _atmWithdrawalsService.GetATMList();
                return View("~/Views/ATMWithdrawals/Create.cshtml", model);
            }
            model.Amount = model.AmountSet;
            var result = await _atmWithdrawalsService.CreateWithdraw(model, user.Result.Id);

            if (!result.Success)
                return View("~/Views/ATMWithdrawals/Create.cshtml", model);

            return View("~/Views/ATMWithdrawals/Index.cshtml", model);
        }
        #endregion

        #region Report Details
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Authorize(Roles = Roles.Administrator)]
        public IActionResult Report()
        {
            return View();
        }


        /// <summary>
        /// Display List of ATM
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<ActionResult> ReportData(Guid userId)
        {
            var atm = await _atmWithdrawalsService.GetReportDetails();
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                switch (sortColumn)
                {
                    case "Name":
                        {
                            atm = sortColumnDirection == "desc" ? atm.OrderByDescending(x => x.ATMName) : atm.OrderBy(x => x.ATMName);
                        }

                        break;
                    case "5000":
                        {
                            atm = sortColumnDirection == "desc" ? atm.OrderByDescending(x => x.Banknotes5000) : atm.OrderBy(x => x.Banknotes5000);
                        }
                        break;
                    case "2000":
                        {
                            atm = sortColumnDirection == "desc" ? atm.OrderByDescending(x => x.Banknotes2000) : atm.OrderBy(x => x.Banknotes2000);
                        }
                        break;
                    case "1000":
                        {
                            atm = sortColumnDirection == "desc" ? atm.OrderByDescending(x => x.Banknotes1000) : atm.OrderBy(x => x.Banknotes1000);
                        }
                        break;
                    case "500":
                        {
                            atm = sortColumnDirection == "desc" ? atm.OrderByDescending(x => x.Banknotes500) : atm.OrderBy(x => x.Banknotes500);
                        }
                        break;
                    case "Date":
                        {
                            atm = sortColumnDirection == "desc" ? atm.OrderByDescending(x => x.CreatedOnUtc) : atm.OrderBy(x => x.CreatedOnUtc);
                        }
                        break;
                }
            }
            else
            {
                atm = atm.OrderByDescending(x => x.CreatedOnUtc);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                atm = atm.Where(m => (m.ATMName != null && m.ATMName.Contains(searchValue))
                                                     || (m.CreatedOnUtc.ToString() != null && m.CreatedOnUtc.ToString().Contains(searchValue)));
            }

            recordsTotal = atm.Count();
            var data = await atm.Skip(skip).Take(pageSize).ToListAsync();
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
        }
        #endregion
    }
}
