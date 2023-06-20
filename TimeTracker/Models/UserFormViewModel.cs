using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models
{
    public class UserFormViewModel
    {
	    public UserFormViewModel() { }

	    public UserFormViewModel(int id, string email)
	    {
            Id = id;
		    Email = email;
	    }

        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
