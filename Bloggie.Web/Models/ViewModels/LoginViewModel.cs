using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password Has To Be atleast 6 characters")]
        public string Password { get; set; }
       
        public string? Returnurl { get; set; }
    }
}
