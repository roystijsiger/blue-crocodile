using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PinkPanther.BlueCrocodile.WebApplication.Areas.Identity.ViewModels;
using PinkPanther.BlueCrocodile.WebApplication.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize(Roles = nameof(Role.Administrator))]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ManageController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index(ManageMessageId? message = null)
        {
            if (message.HasValue)
            {
                ViewData["StatusMessage"] = message.Value;
            }

            var model = _userManager.Users.Select(u => new IndexViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                LockoutEnabled = u.LockoutEnabled,
                AccessFailedCount = u.AccessFailedCount,
                Roles = u.Roles
            });

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid) return View("CreateUser");

            var existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError("UserNameAlreadyExists", "The chosen username already exists.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                Email = model.UserName,
                UserName = model.UserName
            };

            var createUserResult = await _userManager.CreateAsync(user, model.Password);
            if (!createUserResult.Succeeded)
            {
                foreach (var error in createUserResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return View(model);
            }

            var roleName = model.Role.ToString();
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return View("CreateUser");
                }
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!addToRoleResult.Succeeded)
            {
                foreach (var error in addToRoleResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return View("CreateUser");
            }

            return RedirectToAction(nameof(Index), ManageMessageId.CreateUserSuccess);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return RedirectToAction(nameof(Index), ManageMessageId.DeleteUserNotFoundError);

            var result = await _userManager.DeleteAsync(user);
            if(!result.Succeeded) return RedirectToAction(nameof(Index), ManageMessageId.DeleteUserError);

            return RedirectToAction(nameof(Index), ManageMessageId.DeleteUserSuccess);
        }

        public enum ManageMessageId
        {
            DeleteUserNotFoundError,
            DeleteUserError,
            DeleteUserSuccess,
            CreateUserSuccess
        }
    }
}
