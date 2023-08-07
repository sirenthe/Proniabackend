using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pronia.Identity;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class AuthController : Controller

    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel ,string ?returnUrl)
          

        {
          
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
           AppUser appuser =await _userManager.FindByNameAsync(loginViewModel.UsernameorEmail);
            if (appuser == null)
            {
                appuser = await _userManager.FindByEmailAsync(loginViewModel.UsernameorEmail);
                if(appuser == null)
                {
                    ModelState.AddModelError("", "username/email or password is incorrect");
                    return View();
                }
            }

            var signInResult = await _signInManager.PasswordSignInAsync(appuser,
                loginViewModel.Password, loginViewModel.RememberMe, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "get sonra felersen");
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "username/email or password is incorrect");
                return View();
            }

            if(appuser.LockoutEnabled== false)
            {
                appuser.LockoutEnabled = true;
               await  _userManager.UpdateAsync(appuser);
                appuser.LockoutEnd = null;
             
            }

            if (returnUrl is not null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
         



        }



        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
    }
}
}
