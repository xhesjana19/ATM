using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATM.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        /// <summary>
        /// Denied 
        /// </summary>
        /// <returns></returns>
        public IActionResult Denied()
        {
            return View();
        }

        /// <summary>
        /// Page not found
        /// </summary>
        /// <returns></returns>
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}