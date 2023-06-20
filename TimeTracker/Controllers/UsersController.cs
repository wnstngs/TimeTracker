using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;

namespace TimeTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
	        var users = _userManager.Users.Select(u => new UsersListViewModel
	        {
		        UserId = u.Id,
		        UserName = u.UserName,
		        UserEmail = u.Email,
		        UserRoles = string.Join(",", _userManager.GetRolesAsync(u).Result.ToArray())
	        });

			return View(users);
        }

        public IActionResult IndexData()
        {
	        var users = _userManager.Users.Select(u => new UsersListViewModel
	        {
		        UserId = u.Id,
		        UserName = u.UserName,
		        UserEmail = u.Email,
		        UserRoles = string.Join(",", _userManager.GetRolesAsync(u).Result.ToArray())
	        });

	        return PartialView(users);
        }

		[HttpGet]
        public IActionResult Create()
        {
	        return PartialView("Create", new UserFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserFormViewModel user)
        {
	        if (!ModelState.IsValid)
	        {
		        return BadRequest("The user creation form failed to pass validation!");
	        }

			var newUser = await _userManager.Users
		        .Where(x => x.UserName == user.Email)
		        .SingleOrDefaultAsync();
	        if (newUser != null)
	        {
                //
                // There is already a user with that UserName and Email.
                //
		        return BadRequest("There is already a user with that UserName and Email.");
	        }

	        newUser = new ApplicationUser
	        {
                //
                // UserName and Email are always the same for all application users.
                //
		        UserName = user.Email,
		        Email = user.Email
	        };

	        await _userManager.CreateAsync(newUser, user.Password);
	        await _userManager.AddToRoleAsync(newUser, Roles.User);

	        return Ok("User created successfully.");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
	        var user = await _userManager.Users
		        .Where(x => x.Id == id)
		        .SingleOrDefaultAsync();

	        if (user == null)
	        {
		        //
		        // There is no user with that Id.
		        //
		        return BadRequest("There is no user with that Id.");
	        }

	        var model = new UserFormViewModel(user.Id!, user.Email!);

			return PartialView("Edit", model);
        }

        [HttpPost]
        public IActionResult Edit(UserFormViewModel model)
        {
	        return RedirectToAction("Index");
        }

		[HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
	        var user = await _userManager.Users
		        .Where(x => x.Id == id)
		        .SingleOrDefaultAsync();

	        if (user == null)
	        {
		        //
		        // There is no user with that Id.
		        //
		        return BadRequest("There is no user with that Id.");
	        }

	        var model = new UserFormViewModel(user.Id!, user.Email!);

			return PartialView("Delete", model);
        }

		[HttpPost]
		[ActionName("Delete")]
		public async Task<IActionResult> DeletePost(int id)
        {
	        var user = await _userManager.Users
		        .Where(x => x.Id == id)
		        .SingleOrDefaultAsync();

	        if (user == null)
	        {
				//
				// There is no user with that Id.
				//
				return BadRequest("There is no user with that Id.");
	        }

            await _userManager.DeleteAsync(user);

            return Ok("User deleted successfully.");
		}
    }
}