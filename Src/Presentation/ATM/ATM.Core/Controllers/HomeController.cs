using ATM.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ATM.Core.Services.Authentication;
using System.Threading.Tasks;
using ATM.Core.Domain.Users;


namespace ATM.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public HomeController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _authenticationService.GetAuthenticatedUserOrGuestAsync();
            string controllerName = GetControllerName(user.Role);
            if (string.IsNullOrWhiteSpace(controllerName)) return View();
            return RedirectToAction("Index", controllerName);
        }

        [Authorize(Roles = Roles.User)]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = Roles.Administrator)]
        public IActionResult Dashboard()
        {
            return View();
        }

        private string GetControllerName(UserRole userRole)
        {
            if (userRole == UserRole.Administrator) return "User";
            if (userRole == UserRole.User) return "ATMWithdrawals";
            return "";
        }
    }
}
