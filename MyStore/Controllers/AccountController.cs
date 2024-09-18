using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _SignInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
        }

        

        public IActionResult Register ()
        {
            if (_SignInManager.IsSignedIn(User))

            {
                return RedirectToAction("Index", "Home");

            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (_SignInManager.IsSignedIn(User))

            {
                return RedirectToAction("Index", "Home");

            }
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }
            var user = new AppUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                UserName=registerDto.Email,
                CreatedAt = DateTime.Now,
            };
            var result= await _UserManager.CreateAsync(user,registerDto.Password);
            if (result.Succeeded)
            {
                await _UserManager.AddToRoleAsync(user, "client");
                await _SignInManager.SignInAsync(user, false);
                ViewBag.UserName = user.FirstName;
                return RedirectToAction("Index","Store");
                
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View();
        }

        public async Task<IActionResult> Login()
        {
            if (_SignInManager.IsSignedIn(User))

            {
                return RedirectToAction("Index", "Home");

            }
           
            return View() ;

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (_SignInManager.IsSignedIn(User))

            {
                return RedirectToAction("Index", "Home");

            }
            if (!ModelState.IsValid)
            {
                return View(loginDto); 
            }
            var result = await _SignInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password,loginDto.RememberMe,false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = " invaild login attempt";
            }
            return View(loginDto);
            

        }
        public async Task<IActionResult> Logout()
        {
            if (_SignInManager.IsSignedIn(User))
                
            {
                await _SignInManager.SignOutAsync();

            }
            return RedirectToAction("Index", "Home");
        }
        public  IActionResult Profile()
        {
            return View();

        }


    }
}
