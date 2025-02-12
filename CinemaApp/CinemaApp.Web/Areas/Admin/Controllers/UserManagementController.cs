namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using CinemaApp.Common;
    using CinemaApp.Services.Data.Contracts;

    using static Common.ApplicationConstants;
    using System.Data;

    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class UserManagementController(
        IUserService userService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allUsers = await userService.GetAllUsersAsync();

            return View(allUsers);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            bool isGuidValid = ValidationUtils.TryGetGuid(userId, out Guid userGuid);
            if (!isGuidValid)
            {
                return RedirectToAction(nameof(Index));
            }

            bool isUserExisting = await userService.UserExistsByIdAsync(userGuid);
            if (!isUserExisting)
            {
                return RedirectToAction(nameof(Index));
            }

            bool isResultAssigned = await userService.AssignUserToRoleAsync(userGuid, role);
            if (!isResultAssigned)
            {
                return RedirectToAction(nameof(Index));
            }

            //Implement notifications on fail
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            bool isGuidValid = ValidationUtils.TryGetGuid(userId, out Guid userGuid);
            if (!isGuidValid)
            {
                return RedirectToAction(nameof(Index));
            }

            bool isUserExisting = await userService.UserExistsByIdAsync(userGuid);
            if (!isUserExisting)
            {
                return RedirectToAction(nameof(Index));
            }

            bool isResultRemoved = await userService.RemoveUserRoleAsync(userGuid, role);
            if (!isResultRemoved)
            {
                return RedirectToAction(nameof(Index));
            }

            //Implement notifications on fail
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            bool isGuidValid = ValidationUtils.TryGetGuid(userId, out Guid userGuid);
            if (!isGuidValid)
            {
                return RedirectToAction(nameof(Index));
            }

            bool isUserExisting = await userService.UserExistsByIdAsync(userGuid);
            if (!isUserExisting)
            {
                return RedirectToAction(nameof(Index));
            }

            bool isUserRemoved = await userService.DeleteUserAsync(userGuid);
            if (!isUserRemoved)
            {
                return RedirectToAction(nameof(Index));
            }

            //Implement notifications on fail
            return RedirectToAction(nameof(Index));
        }
    }
}
