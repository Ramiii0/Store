using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Models;
using System.ComponentModel.DataAnnotations;

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

        public IActionResult Login()
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
        [Authorize]
        public async   Task<IActionResult> Profile()
        {
            var user = await _UserManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var Userdto = new ProfileDto()
            {
                Address = user.Address,
                Email=user.Email, 
                PhoneNumber=user.PhoneNumber,
                FirstName=user.FirstName,
                LastName=user.LastName
            };
            return View(Userdto);

        }
		[Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileDto profileDto)
        {
            if (!ModelState.IsValid)
            {
				ViewBag.ErrorMessage = "Please Fill all fields with valid";

			}
			var user = await _UserManager.GetUserAsync(User);
			if (user == null)
			{
				return RedirectToAction("Index", "Home");
			}
            user.FirstName = profileDto.FirstName;
            user.LastName = profileDto.LastName;
            user.Email = profileDto.Email;
            user.PhoneNumber = profileDto.PhoneNumber;
            user.Address = profileDto.Address;
            var result = await _UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                ViewBag.SuccessMessage = "Profile Updated Successfuly";
            }
            else
            {
                ViewBag.ErrorMessage = " Error " + result.Errors.First().Description; 

			}
			return View(profileDto);
        }
        [Authorize]
        public IActionResult ResetPassword()
        {
            return View();
        }
		[Authorize]
        [HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
		{
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user= await _UserManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var result = await _UserManager.ChangePasswordAsync(user,resetPasswordDto.CurrentPassword,resetPasswordDto.Password);
            if (result.Succeeded)
            {
                ViewBag.SuccessMessage = "password updated";
            }
            else
            {
				ViewBag.ErrorMessage = " Error " + result.Errors.First().Description;
			}
			return View();
		}
        public IActionResult ForgotPassword()
        {
            if (_SignInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View() ;
        }
        [HttpPost]
		public async Task<IActionResult> ForgotPassword([Required,EmailAddress]string email)
		{
			if (_SignInManager.IsSignedIn(User))
			{
				return RedirectToAction("Index", "Home");
			}
            ViewBag.Email = email;
            if (!ModelState.IsValid)
            {
                ViewBag.EmailError= ModelState["email"].Errors.First().ErrorMessage;
            }
            var user=  await _UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token= _UserManager.GeneratePasswordResetTokenAsync(user);
                string ResetUrl = Url.Action("ResetPassword", "Account", new { token }) ?? "Invaild Url";
                Console.WriteLine(" url link :"+ResetUrl);
            }ViewBag.SuccessMessage = "please check ur email";


            return View();
		}


	}
}
