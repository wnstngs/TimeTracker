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
	        var users = _userManager.Users.Select(u => new DataTableUser
            {
                UserId = u.Id,
                UserName = u.UserName,
                UserEmail = u.Email,
                UserRoles = string.Join(",", _userManager.GetRolesAsync(u).Result.ToArray())
            });

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel user)
        {
	        if (!ModelState.IsValid)
	        {
		        return BadRequest();
	        }

	        var newUser = await _userManager.Users
		        .Where(x => x.UserName == user.Email)
		        .SingleOrDefaultAsync();
	        if (newUser != null)
	        {
                //
                // There is already a user with that UserName and Email.
                //
		        return BadRequest();
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

	        return RedirectToAction("Index");
        }

        [HttpPost]
		public async Task<IActionResult> Delete(string id)
        {
	        var userToDelete = await _userManager.Users
		        .Where(x => x.Id == id)
		        .SingleOrDefaultAsync();

	        if (userToDelete == null)
	        {
				//
				// There is no user with that Id.
				//
				return BadRequest();
	        }

            await _userManager.DeleteAsync(userToDelete);

            return RedirectToAction("Index");
		}
    }
}