using ATM.Core;
using ATM.Core.Services.RechargeATM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class RechargeController : Controller
    {
        #region Ctor.
        /// <summary>
        /// Ctor.
        /// </summary>
        private readonly IRechargeService _rechargeService;
        private readonly Core.Services.Authentication.IAuthenticationService _authenticationService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_rechargeService"></param>
        /// <param name="_authenticationService"></param>
        public RechargeController(IRechargeService rechargeService, Core.Services.Authentication.IAuthenticationService authenticationService)
        {
            _rechargeService = rechargeService;
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
        /// Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            // check for permissions
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            if (user.Result.Role.Equals("Administrator"))
            {
                return RedirectToAction("Denied", "Error");
            }
            var model = await _rechargeService.GetATMDetails(id);

            return View("~/Views/Recharge/Details.cshtml", model);
        }

        /// <summary>
        ///  Create an Banknotes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Create(Guid atmId, string name)
        {
            // check for permissions
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            if (user.Result.Role.Equals("Administrator"))
            {
                return RedirectToAction("Denied", "Error");
            }           
            var model = new RechargeModel();
            return View("~/Views/Recharge/Create.cshtml", new RechargeModel() { AtmId = atmId, Name = name});
        }

        /// <summary>
        /// Create an Banknotes
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(RechargeModel model)
        {
            // check for permissions
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            if (user.Result.Role.Equals("Administrator"))
            {
                return RedirectToAction("Denied", "Error");
            }

            if (!ModelState.IsValid)
            {
                return View("~/Views/Recharge/Create.cshtml", model);
            }
            var result = await _rechargeService.CreateRecharge(model, user.Result.Id);

            if (!result.Success)
                return View("~/Views/Recharge/Create.cshtml", model);

            return RedirectToAction("Details", "Recharge", new { id = model.AtmId });  
        }
        #endregion
    }
}
