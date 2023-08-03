using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pronia.Identity;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class AccountController : Controller

    {

        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public AccountController(IMapper mapper, UserManager<AppUser> userManager)
        {
            _userManager= userManager;
            _mapper = mapper;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Register (RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser newuser = _mapper.Map<AppUser>(registerViewModel);
            newuser.IsActive= true;

          IdentityResult identityResult= 
                await _userManager.CreateAsync(newuser, registerViewModel.Password);
            if (identityResult.Succeeded)
            {
                foreach(var error in identityResult.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }




            return Ok("ugurla qeydiyatdan kecdi");


        }
          
    }
}
