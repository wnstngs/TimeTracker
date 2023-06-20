﻿using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models;

public class TimeEntryFormViewModel
{
	public int Id { get; set; }

	public string? UserId { get; set; }

	[Required]
	[Range(0, 23)]
	public int HoursSpent { get; set; }

	[Required]
	[Range(0, 59)]
	public int MinutesSpent { get; set; }

	[Required]
	public string? Comment { get; set; } = default!;

	[Required] 
	public DateTime Date { get; set; }

	[Required] 
	public DateTime Week { get; set; }

	public TimeEntryFormViewModel()
	{
	}
}
