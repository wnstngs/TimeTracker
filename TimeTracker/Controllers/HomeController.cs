using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using AutoMapper;
using TimeTracker.Models;
using TimeTracker.Services.Contracts;
using TimeTracker.Services;
using Microsoft.EntityFrameworkCore;

namespace TimeTracker.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IClosedWeekService _closedWeekService;
		private readonly ITimeEntryService _timeEntryService;

		public HomeController(
			ILogger<HomeController> logger, 
			UserManager<ApplicationUser> userManager,
			IClosedWeekService closedWeekService,
			ITimeEntryService timeEntryService)
		{
			_logger = logger;
			_userManager = userManager;
			_closedWeekService = closedWeekService;
			_timeEntryService = timeEntryService;
		}

		public async Task<IActionResult> Index(string? weekDate, int? userId)
		{
			DateTime? parsedWeekDate;
			if (weekDate != null)
			{
				try
				{
					//
					// Check if the passed weekDate is Monday.
					// If it is not show current week.
					//
					var probe = DateTime.ParseExact(
						weekDate,
						Common.Constants.DateTimeFormatForWeeks,
						CultureInfo.InvariantCulture);
					if (probe.DayOfWeek != DayOfWeek.Monday)
					{
						parsedWeekDate = null;
					}
					else
					{
						parsedWeekDate = DateTime.ParseExact(
							weekDate,
							Common.Constants.DateTimeFormatForWeeks,
							CultureInfo.InvariantCulture);
					}
				}
				catch
				{
					parsedWeekDate = null;
				}
			}
			else
			{
				parsedWeekDate = null;
			}

			var selectedUserId = await EnsureSelectedUserId(userId);
			var week = parsedWeekDate ?? GetCurrentWeek();
			var prevWeek = week.AddDays(-7);
			var nextWeek = week.AddDays(7);

			var previousWeekTimeEntries = _timeEntryService.GetAllByUserAndWeek(selectedUserId, prevWeek);
			var currentWeekTimeEntries = _timeEntryService.GetAllByUserAndWeek(selectedUserId, week);
			var nextWeekTimeEntries = _timeEntryService.GetAllByUserAndWeek(selectedUserId, nextWeek);

			var previousWeekEntries = MapTimeEntriesToTimeEntryViewModels(previousWeekTimeEntries);
			var currentWeekEntries = MapTimeEntriesToTimeEntryViewModels(currentWeekTimeEntries);
			var nextWeekEntries = MapTimeEntriesToTimeEntryViewModels(nextWeekTimeEntries);

			return View(new TimeEntriesListViewModel(
				previousWeekEntries,
				currentWeekEntries,
				nextWeekEntries
			)
			{
				CurrentDayOfWeek = DateTime.Today.DayOfWeek,
				CurrentWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday),
				Week = week,
				IsWeekClosed = _closedWeekService.IsWeekClosed(selectedUserId, week),
				IsPreviousWeekClosed = _closedWeekService.IsWeekClosed(selectedUserId, prevWeek),
				IsNextWeekClosed = _closedWeekService.IsWeekClosed(selectedUserId, nextWeek)
			});
		}

		[HttpPost]
		public async Task<IActionResult> CreateTimeEntry(TimeEntryFormViewModel? model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("CreateTimeEntry: The time entry creation form failed to pass validation!");
			}

			var selectedUserId = await EnsureSelectedUserId(model.UserId);

			var newIssue = new TimeEntry
			{
				UserId = selectedUserId,
				HoursSpent = model.HoursSpent,
				MinutesSpent = model.MinutesSpent,
				Comment = model.Comment,
				Date = model.Date,
				Week = model.Week
			};

			_timeEntryService.Add(newIssue);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			_timeEntryService.Delete(_timeEntryService.FindById(id));

			return RedirectToAction("Index");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(
				new ErrorViewModel
				{
					RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
				});
		}

		private List<TimeEntryViewModel> MapTimeEntriesToTimeEntryViewModels(List<TimeEntry> timeEntries)
		{
			List<TimeEntryViewModel> timeEntryViewModels = new();
			foreach (var timeEntry in timeEntries)
			{
				timeEntryViewModels.Add(new TimeEntryViewModel
				{
					Id = timeEntry.Id,
					UserId = timeEntry.UserId,
					HoursSpent = timeEntry.HoursSpent,
					MinutesSpent = timeEntry.MinutesSpent,
					Comment = timeEntry.Comment,
					Date = timeEntry.Date,
					Week = timeEntry.Week
				});
			}
			return timeEntryViewModels;
		}

		private DateTime GetCurrentWeek()
		{
			var today = DateTime.Today;
			var monday = today.AddDays(-(today.DayOfWeek != (int)DayOfWeek.Sunday ? (int)today.DayOfWeek : 7) + (int)DayOfWeek.Monday);
			return monday;
		}

		private async Task<ApplicationUser> GetCurrentUser()
		{
			return await _userManager.GetUserAsync(HttpContext.User);
		}

		private async Task<int> GetCurrentUserId()
		{
			return (await GetCurrentUser()).Id;
		}

		private async Task<bool> IsAdmin()
		{
			var currentUser = await GetCurrentUser();

			var roles = await _userManager.GetRolesAsync(currentUser);

			return roles.Contains(Roles.Admin);
		}

		private async Task<int> EnsureSelectedUserId(int? userId)
		{
			return userId.HasValue && (await IsAdmin()) ? userId.Value : (await GetCurrentUserId());
		}
	}
}
