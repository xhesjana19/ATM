using ATM.Core;
using ATM.Core.Services.ATMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ATM.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class ATMController : Controller
    {
        #region Ctor.
        /// <summary>
        /// Ctor.
        /// </summary>
        private readonly IATMService _atmService;
        private readonly Core.Services.Authentication.IAuthenticationService _authenticationService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="atmService"></param>
        /// <param name="authenticationService"></param>
        public ATMController(IATMService atmService, Core.Services.Authentication.IAuthenticationService authenticationService)
        {
            _atmService = atmService;
            _authenticationService = authenticationService;
        }
        #endregion

        #region List & Details
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Display List of ATM
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetATMData()
        {
            var atm = await _atmService.GetAllATMs();
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
                            atm = sortColumnDirection == "desc" ? atm.OrderByDescending(x => x.Name) : atm.OrderBy(x => x.Name);
                        }

                        break;
                    case "Created Date":
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
                atm = atm.Where(m => (m.Name != null && m.Name.Contains(searchValue))
                                                     || (m.CreatedOnUtc.ToString() != null && m.CreatedOnUtc.ToString().Contains(searchValue)));
            }

            recordsTotal = atm.Count();
            var data = await atm.Skip(skip).Take(pageSize).ToListAsync();
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
        public IActionResult Create()
        {
            // check for permissions
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            if (user.Result.Role.Equals("Administrator"))
            {
                return RedirectToAction("Denied", "Error");
            }

            var model = new ATMModel();
            return View("~/Views/ATM/Create.cshtml", model);
        }

        /// <summary>
        /// Create an ATM
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(ATMModel model)
        {
            // check for permissions
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            if (user.Result.Role.Equals("Administrator"))
            {
                return RedirectToAction("Denied", "Error");
            }

            if (!ModelState.IsValid)
            {
                return View("~/Views/ATM/Create.cshtml", model);
            }
            var result = await _atmService.CreateATM(model, user.Result.Id);

            if (!result.Success)
                return View("~/Views/ATM/Create.cshtml", model);

            return View("~/Views/ATM/Index.cshtml");
        }

        /// <summary>
        /// Edit an ATM
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // check for permissions
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            if (user.Result.Role.Equals("Administrator"))
            {
                return RedirectToAction("Denied", "Error");
            }
            var model = await _atmService.GetATMDetailsAsync(id);

            return View("~/Views/ATM/Edit.cshtml", model);
        }

        /// <summary>
        /// Edits an ATM
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(ATMModel model, CancellationToken cancellation)
        {
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            // check for permissions
            if (user.Result.Role.Equals("Administrator"))
            {
                return RedirectToAction("Denied", "Error");
            }
            if (!ModelState.IsValid)
                return View("~/Views/ATM/Edit.cshtml", model);

            var result = await _atmService.EditAtm(model, user.Result.Id, cancellation);
            // if it failed
            if (!result.Success)
                return View("~/Views/ATM/Edit.cshtml", model);

            return View("~/Views/ATM/Index.cshtml");
        }

        /// <summary>
        /// Deletes 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
        {
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            // check for permissions
            if (user.Result.Role.Equals("Administrator"))
            {
                return RedirectToAction("Denied", "Error");
            }
            var result = await _atmService.DeleteATM(id, cancellation);

            // if it failed
            if (!result.Success)
            {
                //todo log4net
            }

            return View("~/Views/ATM/Index.cshtml");
        }
        #endregion

        #region Filter <= 5 000 ALL

        /// <summary>
        /// Filter Index
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Filter()
        {
            return View();
        }

        /// <summary>
        /// Display List of ATM
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> FilterAmount()
        {
            var atm = await _atmService.GetFilterAmount();
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
                            atm = sortColumnDirection == "desc" ? atm.OrderByDescending(x => x.Name) : atm.OrderBy(x => x.Name);
                        }

                        break;
                    case "Created Date":
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
                atm = atm.Where(m => (m.Name != null && m.Name.Contains(searchValue))
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
