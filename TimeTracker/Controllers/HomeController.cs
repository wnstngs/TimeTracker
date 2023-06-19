using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using AutoMapper;
using TimeTracker.Models;
using TimeTracker.Services.Contracts;
using TimeTracker.Services;

namespace TimeTracker.Controllers
{
	public class HomeController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IClosedWeekService _closedWeekService;
		private readonly ITimeEntryService _timeEntryService;

		public HomeController(
			IMapper mapper,
			ILogger<HomeController> logger, 
			UserManager<ApplicationUser> userManager,
			IClosedWeekService closedWeekService,
			ITimeEntryService timeEntryService)
		{
			_mapper = mapper;
			_logger = logger;
			_userManager = userManager;
			_closedWeekService = closedWeekService;
			_timeEntryService = timeEntryService;
		}

		public async Task<IActionResult> Index(string? weekDate, string? userId)
		{
			DateTime? parsedWeekDate;
			if (weekDate != null)
			{
				try
				{
					parsedWeekDate = DateTime.ParseExact(
						weekDate,
						Common.Constants.DateTimeFormatForWeeks,
						CultureInfo.InvariantCulture);
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

			var currentWeekEntries = _mapper.Map<List<TimeEntryViewModel>>(currentWeekTimeEntries);
			var previousWeekEntries = _mapper.Map<List<TimeEntryViewModel>>(previousWeekTimeEntries);
			var nextWeekEntries = _mapper.Map<List<TimeEntryViewModel>>(nextWeekTimeEntries);

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

		/*
		[HttpPost]
		[ValidateModel]
		public async Task<IActionResult> CreateTimeEntry([FromBody] TimeEntryFormViewModel model)
		{
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

			var response = new IssueProjectCodeViewModel
			{
				Id = newIssue.Id,
				ProjectCodeExists = projectCodeExists,
				NonExistentProjectCode = nonExistentProjectCode
			};

			return Json(response);
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
		*/

		private DateTime GetCurrentWeek()
		{
			var today = DateTime.Today;
			var monday = today.AddDays(-(today.DayOfWeek != (int)DayOfWeek.Sunday ? (int)today.DayOfWeek : 7) + (int)DayOfWeek.Monday);
			return monday;
		}

		private async Task<ApplicationUser?> GetCurrentUser()
		{
			return await _userManager.GetUserAsync(HttpContext.User);
		}

		private async Task<string?> GetCurrentUserId()
		{
			return (await GetCurrentUser())?.Id;
		}

		private async Task<bool> IsAdmin()
		{
			var currentUser = await GetCurrentUser();

			var roles = await _userManager.GetRolesAsync(currentUser);

			return roles.Contains(Roles.Admin);
		}

		private async Task<string> EnsureSelectedUserId(string? userId)
		{
			if (userId == null || !await IsAdmin())
			{
				return await GetCurrentUserId();
			}
			return userId;
		}
	}
}
