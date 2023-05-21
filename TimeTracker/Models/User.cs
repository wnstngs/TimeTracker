using Microsoft.AspNetCore.Identity;

namespace TimeTracker.Models;

public class User : IdentityUser<int>
{
    public string FullName { get; set; } = string.Empty;

    public bool Disabled { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<ClosedWeek> ClosedWeeks { get; set; }

    public virtual ICollection<IdentityUserRole<int>> Roles { get; }

    public virtual ICollection<IdentityUserClaim<int>> Claims { get; }

    public virtual ICollection<IdentityUserLogin<int>> Logins { get; }

    public DateTime CreatedDate { get; set; }

    public User(
        string fullName, 
        string email,
        bool emailConfirmed = true)
    {
        UserName = fullName;
        FullName = fullName;
        Email = email;
        EmailConfirmed = emailConfirmed;
        ClosedWeeks = new List<ClosedWeek>();
        Roles = new List<IdentityUserRole<int>>();
        Claims = new List<IdentityUserClaim<int>>();
        Logins = new List<IdentityUserLogin<int>>();
        CreatedDate = DateTime.UtcNow;
    }
}