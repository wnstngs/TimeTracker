using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.Models;

public class ClosedWeek
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }

    [Column(TypeName = "Date")]
    public DateTime Week { get; set; }

    public ClosedWeek(int userId, DateTime week)
    {
        UserId = userId;
        ApplicationUser = default!;
        Week = week;
    }
}