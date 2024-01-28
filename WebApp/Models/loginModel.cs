using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	public class loginModel
	{
		[Required(ErrorMessage = "Please enter a username.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Please enter a password.")]
		public string Password { get; set; }
	}
}
