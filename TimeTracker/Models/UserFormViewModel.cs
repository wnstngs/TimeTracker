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

        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress]
        public string Email { get; set; }

		[Required(ErrorMessage = "PasswordRequired")]
		public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPasswordRequired")]
        [Compare("Password", ErrorMessage = "PasswordsDoNotMatch")]
		public string ConfirmPassword { get; set; }
    }
}
