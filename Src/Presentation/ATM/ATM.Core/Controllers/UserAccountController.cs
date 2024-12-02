using ATM.Core;
using ATM.Core.Services.UsersAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ATM.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class UserAccountController : Controller
    {
        #region Ctor.
        /// <summary>
        /// Ctor.
        /// </summary>
        private readonly IUserAccountService _userAccountService;
        private readonly Core.Services.Authentication.IAuthenticationService _authenticationService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authenticationService"></param>
        /// <param name="userAccountService"></param>
        public UserAccountController(Core.Services.Authentication.IAuthenticationService authenticationService, IUserAccountService userAccountService)
        {
            _authenticationService = authenticationService;
            _userAccountService = userAccountService;
        }
        #endregion

        #region CRUD Operation 
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add Amount
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Create(Guid id)
        {
            // check for permissions
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            if (user.Result.Role.Equals("Administrator"))
            {
                return RedirectToAction("Denied", "Error");
            }
            var result = await _userAccountService.GetUserAccount(id);
            return View("~/Views/UserAccount/Create.cshtml", result);
        }

        /// <summary>
        /// Create 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(UserAccountModel model)
        {
            // check for permissions
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            if (user.Result.Role.Equals("Administrator"))
            {
                return RedirectToAction("Denied", "Error");
            }

            if (!ModelState.IsValid)
            {
                return View("~/Views/UserAccount/Create.cshtml", model);
            }
            var result = await _userAccountService.AddUserAmount(model, user.Result.Id);

            if (!result.Success)
                return View("~/Views/UserAccount/Create.cshtml");

            return View("~/Views/User/Index.cshtml");
        }
        #endregion
    }
}
