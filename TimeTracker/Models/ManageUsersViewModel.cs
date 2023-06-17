namespace TimeTracker.Models;

public class ManageUsersViewModel
{
    public ApplicationUser[] Admins { get; set; }

    public ApplicationUser[] Everyone { get; set; }
}