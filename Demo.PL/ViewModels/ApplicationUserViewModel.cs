using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class ApplicationUserViewModel
    {
        [Required(ErrorMessage = "First name is Required")]
        public string FName { get; set; }
        [Required(ErrorMessage = "Last name is Required")]
        public string LName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "ConfirmPassword is Required")]
        [Compare("Password" ,ErrorMessage = "Password Dosen't Match")]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}
