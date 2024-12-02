using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ATM.Core.Services.Authentication;
using ATM.Core.Data;
using ATM.Core.Domain.Users;
using ATM.Core.Services.Cryptography;
using ATM.Web.Infrastructure;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ATM.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IRepository<User> _userRepository;
        private readonly IEncryptionService _encryptionService;

        public AccountController(IAuthenticationService authenticationService, IRepository<User> userRepository, IEncryptionService encryptionService)
        {
            _authenticationService = authenticationService;
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AuthenticationRequest request, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrWhiteSpace(request.Username) || String.IsNullOrWhiteSpace(request.Password))
            {
                return View();
            }

            var password = _encryptionService.CreatePasswordHash(request.Password);
            var user =   _userRepository.Table.Where(x => x.Username == request.Username && x.Password == password && x.IsDeleted == false).FirstOrDefault();
         
           

            if (user == null)
            {
                ModelState.AddModelError("Username", "Email ose fjalëkalim jo i saktë.");
                return View();
            }

            var result = await _authenticationService.SignInAsync(user, true, cancellationToken);

            if (!result.Success)
            {
                ModelState.AddModelErrorFromResults(result);
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LogOut()
        {
            await _authenticationService.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
