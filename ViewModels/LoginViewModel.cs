using System.ComponentModel.DataAnnotations;

namespace MakeupStore.ViewModels
{
    // view model pentru formularul de login
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        // url-ul de redirectionare dupa login
        public string? ReturnUrl { get; set; }
    }
}
