using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Store.Models.Models;
using System.Drawing.Printing;
using System.Linq;

namespace MyStore.Controllers
{
    [Authorize(Roles ="admin")]
    [Route("/admin/[controller]/{action=Index}/{id?}")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly int pageSize = 5;

        public UsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager )
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index(int pageIndex)
        {
            IQueryable<AppUser> query = _userManager.Users.OrderByDescending(x => x.CreatedAt);
            if (pageIndex == null || pageIndex <1)
            {
                pageIndex = 1;
            }
            decimal count = query.Count();
            int totalPages= (int)Math.Ceiling(count/pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var users =query .ToList();
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = totalPages;
            return View(users);
        }
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Roles= await _userManager.GetRolesAsync(user);
            var AvailableRoles= await _roleManager.Roles.ToListAsync();
            var items= new List<SelectListItem>();
            foreach (var role in AvailableRoles) {
                items.Add(new SelectListItem
                {
                    Text = role.NormalizedName,
                    Value = role.Name,
                    Selected = await _userManager.IsInRoleAsync(user, role.Name)
                });
            }
            ViewBag.SelectItems = items;
            return View(user);

        }
        public async Task<IActionResult> EditRole(string id,string newrole)
        {
            if (id == null || newrole == null)
            {
                return RedirectToAction("Details");
            }
            var user= await _userManager.FindByIdAsync(id);
            var Role= await _roleManager.RoleExistsAsync(newrole);
            if (user == null || Role == null)
            {

                return RedirectToAction("Details",new {id});
            }
            var currentUser= await _userManager.GetUserAsync(User);
            if (currentUser.Id == user.Id)
            {
                TempData["ErrorMessage"] = "you can not update ur own role";
                return RedirectToAction("Details", new { id });
            }
            var userRole = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user,userRole);
            await _userManager.AddToRoleAsync(user, newrole);
			TempData["SuccessMessage"] = "role updated successfuly";
			return RedirectToAction("Details",new {id});


        }
        public async Task<IActionResult> DeleteAccount(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessDelete"] = "Account deleted successfully";

				return RedirectToAction("Index");
            }
            else
            {
				TempData["ErrorMessage"] = " Error " + result.Errors.First().Description;
				return RedirectToAction("Index",new {id});
			}
            
        }
    }
}
