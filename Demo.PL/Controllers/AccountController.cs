using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
		    _signInManager = signInManager;
		}
        //regester
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(ApplicationUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
					FName = model.FName,
					Email = model.Email,
					LName = model.LName,
                    isAgree = model.IsAgree,
				};
                var res = await _userManager.CreateAsync(user ,model.Password);
                if (res.Succeeded)
                {
                    return RedirectToAction("LogIn");
                }
                else
                {
					foreach (var erorr in res.Errors)
                        ModelState.AddModelError(string.Empty, erorr.Description);
                    
                }
            }
                return View(model);
        }
        //log in
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> LogInAsync(LogInViewModel modle)
		{
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(modle.Email);
                if (User is not null)
                {

                    var loginRes = await _signInManager.PasswordSignInAsync(User, modle.Password, modle.RememberMe, false);
                    if (loginRes.Succeeded)
                    {
                        return RedirectToAction("Index" , "Home");
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Password is Incorrect");
                }
                else
                    ModelState.AddModelError(string.Empty, "Email is not Exists");
            }
            return View(modle);
               
		}
		//sign out
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LogIn");
        }
		//forget password
		//reset password
	}
}
