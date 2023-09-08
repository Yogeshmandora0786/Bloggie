using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Bloggie.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

            if (identityResult.Succeeded)
            {
                //Assign User Role

                var roleidentity = await userManager.AddToRoleAsync(identityUser, "User");

                if (roleidentity.Succeeded)
                {
                    //Show Success Message
                    return RedirectToAction("Register");
                }
            }
            //Show Error Message
            return View();
        }

        [HttpGet]
        public IActionResult Login(string Returnurl)
        {
            var model = new LoginViewModel
            {
                Returnurl = Returnurl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var signinresult = await signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);

            if (signinresult != null && signinresult.Succeeded)
            {
                if(!string.IsNullOrWhiteSpace(loginViewModel.Returnurl))
                {
                    return Redirect(loginViewModel.Returnurl);
                }

                //Show Success 
                return RedirectToAction("Index", "Home");
            }

            //Show Errors
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
