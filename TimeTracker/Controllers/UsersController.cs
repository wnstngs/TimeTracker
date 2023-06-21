using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;
using TimeTracker.Services.Contracts;

namespace TimeTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        public UsersController(
	        UserManager<ApplicationUser> userManager,
	        IUserService userService
        )
        {
            _userManager = userManager;
			_userService = userService;
        }

        public IActionResult Index()
        {
	        return View(GetUsersWithRoles());
        }

        public IActionResult IndexData()
        {
	        return PartialView(GetUsersWithRoles());
        }

		[HttpGet]
        public IActionResult Create()
        {
	        return PartialView("Create", new UserFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserFormViewModel model)
        {
	        if (!ModelState.IsValid)
	        {
		        return BadRequest("The user creation form failed to pass validation!");
	        }

	        if (_userService.UserExists(model.Email))
	        {
		        return BadRequest("There is already a user with that UserName and Email.");
	        }

	        var newUser = new ApplicationUser
	        {
                //
                // UserName and Email are always the same for all application users.
                //
		        UserName = model.Email,
		        Email = model.Email
	        };

	        await _userManager.CreateAsync(newUser, model.Password);
	        await _userManager.AddToRoleAsync(newUser, Roles.User);

	        return Ok("User has been created.");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
	        var user = _userService.FindById(id);

	        var model = new UserFormViewModel(user.Id, user.Email!);

			return PartialView("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserFormViewModel model)
        {
	        if (!ModelState.IsValid)
	        {
		        return BadRequest("The user update form failed to pass validation!");
	        }

	        var userToEdit = _userService.GetByIdForEdit(model.Id);

			//
			// Check if that user already exists, but ignore the name if it matches the
			// current user name (cases where the user only changes the password without
			// updating the name).
			//
			if (_userService.UserExists(model.Email) && 
	            model.Email != userToEdit.Email)
	        {
		        return BadRequest("There is already a user with that UserName and Email.");
			}

			//
			// The UserName and Email are always the same for all application users.
			//
			userToEdit.Email = model.Email;
			userToEdit.UserName = model.Email;

			var result = _userManager.UpdateAsync(userToEdit).Result;

			if (!result.Succeeded)
			{
				//
				// The user update operation was not successful.
				//
				return BadRequest("User update failed.");
			}

			//
			// Assuming the user update was successful, generate a password reset token for
			// the updated user and reset the user's password.
			//
			var resetToken = _userManager.GeneratePasswordResetTokenAsync(userToEdit).Result;
			result = _userManager.ResetPasswordAsync(userToEdit, resetToken, model.Password).Result;
			
			if (result.Succeeded)
			{
				//
				// Return Ok if the password reset operation was successful.
				//
				return Ok("User has been updated");
			}

			return BadRequest("User update failed.");
		}

		[HttpGet]
        public IActionResult Delete(int id)
        {
	        var user = _userService.FindById(id);
	        return PartialView("Delete", new UserFormViewModel(user.Id, user.Email!));
        }

		[HttpPost]
		[ActionName("Delete")]
		public async Task<IActionResult> DeletePost(int id)
        {
	        var user = _userService.FindById(id);

	        await _userManager.DeleteAsync(user);

            return Ok("User has been deleted.");
		}

		private IQueryable<UsersListViewModel> GetUsersWithRoles()
		{
			var users = _userManager.Users.Select(u => new UsersListViewModel
			{
				UserId = u.Id,
				UserName = u.UserName,
				UserEmail = u.Email,
				UserRoles = string.Join(",", _userManager.GetRolesAsync(u).Result.ToArray())
			});
			return users;
		}
    }
}