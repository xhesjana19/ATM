using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ATM.Core;
using ATM.Core.Data;
using ATM.Core.Services.Users;
using ATM.Core.Services.Authentication;
using ATM.Core.Results;
using ATM.Core.Services.UsersAccount;

namespace ATM.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class UserController : Controller
    {
        #region Ctor.
        /// <summary>
        /// Ctor.
        /// </summary>
        private readonly IUserService _userService;
        private readonly IUserAccountService _userAccountService;
        private readonly IAuthenticationService _authenticationService;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="userAccountService"></param>
        /// <param name="authenticationService"></param>
        public UserController(IUserService userService,IUserAccountService userAccountService, Core.Services.Authentication.IAuthenticationService authenticationService)
        {
            _userService = userService;
            _userAccountService = userAccountService;
            _authenticationService = authenticationService;
  
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> GetUserData()
        {
            var userModel =  _userService.GetUsersWithoutPassword();
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
                            userModel = sortColumnDirection == "desc" ? userModel.OrderByDescending(x => x.Name) : userModel.OrderBy(x => x.Name);
                        }
                        break;
                    case "Email":
                        {
                            userModel = sortColumnDirection == "desc" ? userModel.OrderByDescending(x => x.Username) : userModel.OrderBy(x => x.Username);
                        }
                        break;
                }
            }
            else
            {
                userModel = userModel.OrderByDescending(x => x.Name);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                userModel = userModel.Where(m => (m.Name != null && m.Name.Contains(searchValue))
                                                     || (m.Username != null && m.Username.Contains(searchValue))
                                                   );
            }

            recordsTotal = userModel.Count();
            var data = await userModel
                .Skip(skip).Take(pageSize).ToListAsync();
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
        }

        #region CRUD operations

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateUserRequest()
            {
                Username= " ",
                Password = " ",
                ConfirmPassword = " "
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var user = await _userService.GetUserByIdAsync(Id.ToString());
          
            return View(GetNewUpdateUserRequestFromUserModel(user));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> EditCredentials()
        {
            var authenticatedUser = await _authenticationService.GetAuthenticatedUserOrGuestAsync();
            return View(GetEditUserCredentialsRequestFromUserModel(authenticatedUser));
        }


        [AcceptVerbs("Post")]
        public async Task<IActionResult> CreateUser(CreateUserRequest user, CancellationToken cancellationToken = default)
        {
            var authenticatedUser = await _authenticationService.GetAuthenticatedUserOrGuestAsync();
            user.CreatedBy = authenticatedUser.Id;

            if (!ModelState.IsValid) return View("Create", user);

           var result = await DoOperation(user, _userService.CreateUserAsync, cancellationToken);
          
            if (!result.Success)
                return View("Index");

            return View("Index");

        }

        public async Task<IActionResult> UpdateUser(UpdateUserRequest user, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return View("Edit", user);

           var result =  await DoOperation(user, _userService.UpdateUserAsync, cancellationToken);

            var authenticatedUser = await _authenticationService.GetAuthenticatedUserOrGuestAsync();

            if (!result.Success)
                return View("Edit");

            return View("Index");
        }

        [AllowAnonymous]
        public async Task<IActionResult> EditUserCredentials(EditUserCredentialsRequest user, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return View("EditCredentials", user);

            await DoOperation(user, _userService.EditUserCredentialsAsync, cancellationToken);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellation)
        {
            var user = _authenticationService.GetAuthenticatedUserOrGuestAsync();
            // check for permissions
            if (user.Result.Role.Equals("Administrator"))
            {
                return RedirectToAction("Denied", "Error");
            }
            var result = await _userService.DeleteUserAsync(id, cancellation);
                if (!result.Success)
                return View("Index");

            return View("Index");
        }

        #endregion

        private UpdateUserRequest GetNewUpdateUserRequestFromUserModel(UserModel user)
        {
            return new UpdateUserRequest
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Password = user.Password,
               
                Role = user.Role
            };
        }

        private EditUserCredentialsRequest GetEditUserCredentialsRequestFromUserModel(AuthenticatedUser user)
        {
            return new EditUserCredentialsRequest
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                NewPassword = "",
                ConfirmPassword = ""
            };
        }
        private async Task<Result> DoOperation<T>(
      T model,
      Func<T, CancellationToken, Task<Result>> operation,
      CancellationToken cancellationToken = default)
        {
            var result = await operation(model, cancellationToken);

            if (!result.Success)
            {
                //ModelState.AddModelError(result);
            }
            return result;
        }
    }
}
